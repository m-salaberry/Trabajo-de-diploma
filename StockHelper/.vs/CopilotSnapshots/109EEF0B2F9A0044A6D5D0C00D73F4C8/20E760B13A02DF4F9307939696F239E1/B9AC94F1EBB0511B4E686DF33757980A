using Services.Contracts;
using Services.Contracts.CustomsException;
using Services.DAL.Implementations.Repositories;
using Services.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Transactions;

namespace Services.Implementations
{
    /// <summary>
    /// Service for managing permissions (Patents and Families).
    /// Implements Singleton pattern with caching for improved performance.
    /// </summary>
    public class PermissionService : IGenericService<Component>
    {
        private PatentRepository _patentRepository;
        private FamilyRepository _familyRepository;
        
        private static PermissionService _instance = null;
        private static readonly object _lock = new object();

        private Dictionary<Guid, Component> _cachedPermissions = null;
        private List<Patent> _cachedPatents = null;
        private List<Family> _cachedFamilies = null;
        private DateTime? _lastCacheUpdate = null;
        private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(30);

        /// <summary>
        /// Private constructor to enforce Singleton pattern.
        /// </summary>
        private PermissionService()
        {
            _patentRepository = new PatentRepository();
            _familyRepository = new FamilyRepository();
        }

        /// <summary>
        /// Gets the singleton instance of PermissionService.
        /// Thread-safe implementation using double-check locking.
        /// </summary>
        /// <returns>Singleton instance</returns>
        public static PermissionService Instance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new PermissionService();
                    }
                }
            }
            return _instance;
        }

        /// <summary>
        /// Gets all components (both patents and families) with caching.
        /// </summary>
        /// <returns>List of all components</returns>
        public List<Component> GetAll()
        {
            // Check if cache is valid
            if (_cachedPermissions == null ||
                _lastCacheUpdate == null ||
                DateTime.Now - _lastCacheUpdate > _cacheExpiration)
            {
                RefreshCache();
            }

            return _cachedPermissions.Values.ToList();
        }

        /// <summary>
        /// Gets all patents with caching.
        /// </summary>
        /// <returns>List of all patents</returns>
        public List<Patent> GetAllPatents()
        {
            // Check if cache is valid
            if (_cachedPatents == null ||
                _lastCacheUpdate == null ||
                DateTime.Now - _lastCacheUpdate > _cacheExpiration)
            {
                RefreshCache();
            }

            return _cachedPatents;
        }

        /// <summary>
        /// Gets all families (roles) with caching.
        /// </summary>
        /// <returns>List of all families</returns>
        public List<Family> GetAllFamilies()
        {
            // Check if cache is valid
            if (_cachedFamilies == null ||
                _lastCacheUpdate == null ||
                DateTime.Now - _lastCacheUpdate > _cacheExpiration)
            {
                RefreshCache();
            }

            return _cachedFamilies;
        }
        /// <summary>
        /// Retrieves a family entity with the specified name.
        /// </summary>
        /// <param name="name">The name of the family to retrieve. Cannot be null, empty, or consist only of white-space characters.</param>
        /// <returns>A <see cref="Family"/> object that matches the specified name, or <see langword="null"/> if no such family
        /// exists.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> is null, empty, or consists only of white-space characters.</exception>
        public Family GetFamilyByName(string name)
        {
            // Check if name is valid
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Family name cannot be null or empty", nameof(name));
            }
            return _familyRepository.GetByName(name);
        }

        public IEnumerable<Patent> GetFamilyPatents(Guid familyId)
        {
            return _familyRepository.GetFamilyPatents(familyId);
        }

        /// <summary>
        /// Refreshes the cache by loading data from repositories.
        /// </summary>
        private void RefreshCache()
        {
            _cachedPatents = _patentRepository.GetAll().ToList();
            _cachedFamilies = _familyRepository.GetAll().ToList();

            // Initialize dictionary and combine both patents and families
            _cachedPermissions = new Dictionary<Guid, Component>();
            
            foreach (var patent in _cachedPatents)
            {
                _cachedPermissions[patent.Id] = patent;
            }
            
            foreach (var family in _cachedFamilies)
            {
                _cachedPermissions[family.Id] = family;
            }

            _lastCacheUpdate = DateTime.Now;
        }

        /// <summary>
        /// Invalidates the cache, forcing a reload on next access.
        /// Call this after modifying permissions or roles in the database.
        /// </summary>
        public void InvalidateCache()
        {
            _cachedPermissions = null;
            _cachedPatents = null;
            _cachedFamilies = null;
            _lastCacheUpdate = null;
        }

        public Component GetById(Guid id)
        {
            // Check if cache is valid
            if (_cachedPermissions == null ||
                _lastCacheUpdate == null ||
                DateTime.Now - _lastCacheUpdate > _cacheExpiration)
            {
                RefreshCache();
            }

            // Fast lookup using dictionary
            return _cachedPermissions.GetValueOrDefault(id);
        }

        public void Insert(Component entity)
        {
            if (entity is Patent patent)
            {
                _patentRepository.Create(patent);
            }
            else if (entity is Family family)
            {
                this.InsertNewFamily(family);
            }
            else
            {
                throw new ArgumentException($"Entity must be of type Patent or Family, but was {entity.GetType().Name}");
            }

            // Invalidate cache after insert
            InvalidateCache();
        }

        public void Update(Component entity)
        {
            if (entity is Patent patent)
            {
                _patentRepository.Update(patent);
            }
            else if (entity is Family family)
                { 
                List<Guid> currentPatentsId = _familyRepository.GetById(family.Id).Children.Select(c => c.Id).ToList();
                List<Guid> newPatentsId = family.Children.Select(c => c.Id).ToList();

                List<Guid> toAdd = newPatentsId.Except(currentPatentsId).ToList();
                List<Guid> toRemove = currentPatentsId.Except(newPatentsId).ToList();

                using(TransactionScope scope = new TransactionScope())
                {
                    foreach(var patentToAdd in toAdd)
                    {
                        _familyRepository.AssignPatentToFamily(patentToAdd, family.Id);
                    }
                    foreach(var patentToRemove in toRemove)
                    {
                        _familyRepository.RemovePatentFromFamily(patentToRemove, family.Id);
                    }
                    _familyRepository.Update(family);
                    scope.Complete();
                }
            }
            else
            {
                throw new ArgumentException($"Entity must be of type Patent or Family, but was {entity.GetType().Name}");
            }

            // Invalidate cache after update
            InvalidateCache();
        }

        public void Delete(Guid id)
        {
            var family = _familyRepository.GetById(id);
            // Verify family exists
            if (family == null)
            {
                throw new KeyNotFoundException($"Component with ID {id} not found");
            }
            // Check if any user is assigned to this family
            List<User> usersWithFemilyAssigned = this.IsUserAssignedToFamily(family.Name);
            if (usersWithFemilyAssigned.Count != 0)
            {
                string userNames = string.Join("\n - ", usersWithFemilyAssigned.Select(u => u.Name));
                throw new MySystemException($"Cannot delete family {family.Name} assigned to user/s:\n" + userNames, "BLL");
            }
            using(TransactionScope trx = new TransactionScope())
            {
                
                foreach (var child in family.Children)
                {
                    _familyRepository.RemovePatentFromFamily(child.Id, family.Id);
                }
                _familyRepository.Delete(id);
                trx.Complete();
            }

            InvalidateCache();
        }

        public bool Exists(Guid id)
        {
            var component = GetById(id);
            return component != null;
        }

        /// <summary>
        /// Inserts a new family into the system and associates its children with the family.
        /// </summary>
        /// <param name="family">The family to insert. The object must not be null and must contain valid child references.</param>
        /// <exception cref="MySystemException">Thrown if the family is null or if any child referenced by the family does not exist in the system.</exception>
        public void InsertNewFamily(Family family)
        {
            if (family == null)
            {
                throw new MySystemException(nameof(family) + ": Family cannot be null", "BLL");
            }
            family.Id = GenerateUniqueGuid();

            _familyRepository.Create(family);

            foreach (var child in family.Children)
            {
                if (!Exists(child.Id))
                {
                    throw new MySystemException("Child patent with ID " + child.Id + " does not exist", "BLL");
                }
                _familyRepository.AssignPatentToFamily(child.Id, family.Id);
            }
        }

        /// <summary>
        /// Determines whether any user is assigned to the specified family.
        /// </summary>
        /// <param name="familyName">The .</param>
        /// <returns>true if at least one user is assigned to the specified family; otherwise, false.</returns>
        private List<User> IsUserAssignedToFamily(string familyName)
        {
            List<User> users = UserService.Instance().GetAll();
            List<User> usersWithFamilyAssigned = new List<User>();
            foreach (var user in users)
            {
                if (user.Role == familyName)
                {
                    usersWithFamilyAssigned.Add(user);
                }
            }
            return usersWithFamilyAssigned;
        }

        /// <summary>
        /// Generates a unique GUID that doesn't exist in the database.
        /// While GUID collisions are extremely rare, this ensures database integrity.
        /// </summary>
        /// <returns>A unique GUID</returns>
        private Guid GenerateUniqueGuid()
        {
            Guid newGuid;
            int maxAttempts = 10; // Límite de intentos por seguridad
            int attempts = 0;

            do
            {
                newGuid = Guid.NewGuid();
                attempts++;

                if (attempts >= maxAttempts)
                {
                    throw new MySystemException(
                        "Unable to generate unique GUID after multiple attempts",
                        "BLL");
                }
            }
            while (Exists(newGuid));

            return newGuid;
        }
    }
}

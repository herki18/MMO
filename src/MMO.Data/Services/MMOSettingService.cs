using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MMO.Data.Entities;

namespace MMO.Data.Services
{
    public class MMOSettingService {
        private const string EnabledGameRolesKey = "Enabled Game Roles";
        private readonly MMODatabseContext _database;
        private readonly Dictionary<string, MMOSetting> _settingEntities; 
        private HashSet<Role> _gameRoles;
 
        public IEnumerable<Role> EnabledGameRoles { get { return _gameRoles; } } 

        public MMOSettingService(MMODatabseContext database) {
            _gameRoles = new HashSet<Role>();
            _database = database;
            _settingEntities = new Dictionary<string, MMOSetting>();
            LoadAllSettings();
        }

        public bool IsGameEnabledForuser(User user) {
            return _gameRoles.Overlaps(user.Roles);
        }

        public bool IsGameEnabledForRole(Role role) {
            return _gameRoles.Contains(role);
        }

        public void EnabledGameForRoles(Role role) {
            if(!_gameRoles.Add(role));
            {
                return;
            }

            SaveRolesToDatabase();
        }

        public void DisableGameForRoles(Role role) {
            if (!_gameRoles.Remove(role)) {
                return;
            }

            SaveRolesToDatabase();
        }

        public void SetEnabledGameRoles(IEnumerable<Role> roles) {
            _gameRoles = new HashSet<Role>(roles);
            SaveRolesToDatabase();
        }

        public void LoadAllSettings() {
            foreach (var setting in _database.MmoSettings) {
                _settingEntities.Add(setting.Key, setting);
            }

            MMOSetting enabledGameRolesSetting;
            if (_settingEntities.TryGetValue(EnabledGameRolesKey, out enabledGameRolesSetting)) {
                var roleIds = new List<int>();

                foreach (var roleIdSring in enabledGameRolesSetting.Value.Split(',')) {
                    int roleId;
                    if (!int.TryParse(roleIdSring, out roleId)) {
                        continue;
                    }

                    roleIds.Add(roleId);
                }

                foreach (var role in _database.Roles.Where(t => roleIds.Contains(t.Id))) {
                    _gameRoles.Add(role);
                }
            }
        }

        private void SaveRolesToDatabase() {
            

            MMOSetting enabledGameRoleSetting;
            if (!_settingEntities.TryGetValue(EnabledGameRolesKey, out enabledGameRoleSetting)) {
                enabledGameRoleSetting = _settingEntities[EnabledGameRolesKey] = new MMOSetting() {
                    Key = EnabledGameRolesKey
                };

                _database.MmoSettings.Add(enabledGameRoleSetting);
            }

            enabledGameRoleSetting.Value = string.Join(",", _gameRoles.Select(t => t.Id));

            _database.SaveChanges();
        }
    }
}

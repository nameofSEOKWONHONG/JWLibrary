using JWLibrary.Core;
using LiteDbFlex;
using System;
using System.Collections.Generic;
using System.Text;

namespace JWLibrary.Version {
    public class ConfigManager {
        public ConfigManager() {
        }
        public ConfigInfo GetInitialConfig(string ovnerName, Dictionary<string, string> etcs = null) {
            if (ovnerName.jIsNullOrEmpty()) throw new Exception("ownerName is null.");
            var result = LiteDbFlexerManager.Instance.Value.Create<ConfigInfo>().EnsureIndex(x => x.OwnerName).Get(x => x.OwnerName == ovnerName).GetResult<ConfigInfo>();
            if(result.jIsNull()) {
                result = new ConfigInfo() {
                    OwnerName = ovnerName,
                    OwnerOldVersion = 0,
                    OwnerNowVersion = 1,
                    Datum = etcs
                };
                LiteDbFlexerManager.Instance.Value.Create<ConfigInfo>().Insert(result);
            }
            else {
                result.OwnerOldVersion = result.OwnerNowVersion;
                result.OwnerNowVersion = result.OwnerNowVersion + 1;
                if (etcs.jIsNotNull())
                    result.Datum = etcs;

                LiteDbFlexerManager.Instance.Value.Create<ConfigInfo>().Update(result);
            }

            return result;
        }

        public ConfigInfo GetConfig(string ownerName) {
            return LiteDbFlexerManager.Instance.Value.Create<ConfigInfo>().EnsureIndex(x => x.OwnerName).Get(x => x.OwnerName == ownerName).GetResult<ConfigInfo>();
        }

        [LiteDbTable("config.db", "configInfos")]
        public class ConfigInfo {
            public int Id { get; set; } = 0;
            public string OwnerName { get; set; } = string.Empty;
            public int OwnerNowVersion {
                get {
                    return OwnerOldVersion + 1;
                }
                set {
                    value = value + OwnerOldVersion;
                }
            }
            public int OwnerOldVersion { get; set; } = 0;
            public Dictionary<string, string> Datum { get; set; } = new Dictionary<string, string>();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace JWLibrary.Core {
    public static class JMapper {

        #region [object deep copay]

        public static TDest jMapped<TSrc, TDest>(this TSrc src)
            where TSrc : class, new()
            where TDest : class, new() {
            var tdest = new TDest();

            var tsrcProperties = src.GetType().GetProperties();
            var tdestProperties = tdest.GetType().GetProperties();

            foreach (var srcProperty in tsrcProperties) {
                foreach (var tdestProperty in tdestProperties) {
                    if (srcProperty.Name == tdestProperty.Name && srcProperty.PropertyType == tdestProperty.PropertyType) {
                        tdestProperty.SetValue(tdest, srcProperty.GetValue(src));
                        break;
                    }
                }
            }

            return tdest;
        }

        #endregion [object deep copay]
    }

    public static class JCastExtension {
        public static TDest jCast<TDest>(this object src) 
            where TDest : class {
            return src as TDest;
        }
        public static TDest jCast<TSrc, TDest>(this TSrc src)
            where TSrc : class
            where TDest : class {
            return src as TDest;
        }
    }
}

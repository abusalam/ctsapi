using System.Reflection;

namespace CTS_BE.Helper
{
    public static class ExtMapper
    {
        public static void FillFrom<TDst, TSrc>(this TDst target, TSrc source)
        {
            if(source is null) {
                return;
            }
            if(target is null) {
                return;
            }
            PropertyInfo? targetPropInfoByName;
            string sourcePropertyName;
            object? sourcePropValue;
            foreach (PropertyInfo sourcePropertyInfo in source.GetType().GetProperties().Where(p => p.CanRead))
            {
                sourcePropertyName = sourcePropertyInfo.Name;
                targetPropInfoByName = target.GetType().GetProperty(sourcePropertyName);
                if (targetPropInfoByName is null)
                {
                    continue;
                }
                sourcePropValue = sourcePropertyInfo.GetValue(source, null);
                if( targetPropInfoByName.CanWrite) {
                    // try {
                        targetPropInfoByName.SetValue(target, sourcePropValue, null);
                    // }
                    // catch(ArgumentException) {
                    //     continue;
                    // }
                }
            }
        }
    }
}
using System.Reflection;

namespace CTS_BE.Helper
{
    public static class ExtMapper
    {
        public static void FillFrom<TSrc, TDst>(this TSrc target, TDst source)
        {
            if(source is null) {
                return;
            }
            PropertyInfo? targetPropInfoByName;
            string sourcePropertyName;
            object? sourcePropValue;
            foreach (PropertyInfo sourcePropertyInfo in source.GetType().GetProperties().Where(p => p.CanRead))
            {
                sourcePropertyName = sourcePropertyInfo.Name;
                if(target is not null) {
                    targetPropInfoByName = target.GetType().GetProperty(sourcePropertyName);
                    if (targetPropInfoByName != null)
                    {
                        sourcePropValue = sourcePropertyInfo.GetValue(source, null);
                        if(sourcePropValue != null && targetPropInfoByName.CanWrite) {
                            targetPropInfoByName.SetValue(target, sourcePropValue, null);
                        }
                    }
                }

            }
        }
    }
}
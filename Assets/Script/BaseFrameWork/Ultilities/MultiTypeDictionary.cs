using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiTypeDictionary: IEnumerable<KeyValuePair<object,object>>
{   
    Dictionary<System.Object,System.Object> value;
    MultiTypeDictionary()
    {
        value = new Dictionary<object, object>();
    }

    public T1 GetItem<T,T1>(T key)
    {
        if (key != null)
        {
            return (T1)value[key];
        }
        else
        {
            return default(T1);
        }
    }

    public void SetItem(object key,object value)
    {
        this.value[key] = value;
    }

    public object this[object key]
    {
        get
        {
            if (key != null)
            {
                return value[key];
            }
            else
            {
                return null;
            }
        }
        set
        {
            this.value[key] = value;
        }
    }

    public bool ContainsKey(object key)
    {
        return key != null && value.ContainsKey(key);
    }

    public void Clear()
    {
        value.Clear();
    }

    public IEnumerator<KeyValuePair<object, object>> GetEnumerator()
    {
        return value.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public int Count => value.Count;
}

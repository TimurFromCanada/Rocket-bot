using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace rocket_bot
{
    public class Channel<T> where T : class
    {
        public List<T> List = new List<T>();

        /// <summary>
        /// Возвращает элемент по индексу или null, если такого элемента нет.
        /// При присвоении удаляет все элементы после.
        /// Если индекс в точности равен размеру коллекции, работает как Append.
        /// </summary>
        public T this[int index]
        {
            get
            {
                lock (List)
                {
                    if (List.Count <= index || index < 0)
                    {
                        return null;
                    }

                    return List[index];
                }
            }
            set
            {
                lock (List)
                {
                    if (Count > index && index >= 0)
                    {
                        List[index] = value;
                        List.RemoveRange(index + 1, List.Count - 1 - index);
                    }

                    else if (index == List.Count)
                    {
                        List.Add(value);
                    }
                }
            }
        }

        /// <summary>
        /// Возвращает последний элемент или null, если такого элемента нет
        /// </summary>
        public T LastItem()
        {
            lock (List)
            {
                return List.Count == 0 ? null : List[List.Count - 1];
            }
        }

        /// <summary>
        /// Добавляет item в конец только если lastItem является последним элементом
        /// </summary>
        public void AppendIfLastItemIsUnchanged(T item, T knownLastItem)
        {
            lock (List)
            {
                if (Equals(LastItem(), knownLastItem))
                {
                    List.Add(item);
                }
            }
        }

        /// <summary>
        /// Возвращает количество элементов в коллекции
        /// </summary>
        public int Count
        {
            get
            {
                lock (List)
                {
                    return List.Count;
                }
            }
        }
    }
}
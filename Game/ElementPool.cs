using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class StaticElementPool<T1, T>
    {
        private Queue<T> _elements;
        private Func<T1, T> _elementGenerator;

        private int _size;
        private int _capacity;

        public StaticElementPool(Func<T1, T> elementGenerator, int p_poolSize)
        {
            _elements = new Queue<T>();
            _elementGenerator = elementGenerator;
            _size = p_poolSize;
        }

        public T GetElement(T1 id)
        {
            T item = default;

            if (_elements.Count > 0)
            {
                item = _elements.Dequeue();
            }
            else
            {
                if (_capacity < _size)
                {
                    item = _elementGenerator(id);
                    _capacity++;
                }
            }

            return item;
        }

        public void ReleaseObject(T item)
        {
            _elements.Enqueue(item);
        }
    }

    public class ElementPool<T1, T>
    {
        private Queue<T> _elements;
        private Func<T1, T> _elementGenerator;

        public ElementPool(Func<T1, T> elementGenerator)
        {
            _elements = new Queue<T>();
            _elementGenerator = elementGenerator;
        }

        public T GetElement(T1 id)
        {
            T item;

            if (_elements.Count > 0)
            {
                item = _elements.Dequeue();
                Console.WriteLine("Re-utilizando elemento");
            }
            else
            {
                item = _elementGenerator(id);
                Console.WriteLine("Creando elemento");
            }

            return item;
        }

        public void ReleaseObject(T item)
        {
            _elements.Enqueue(item);
        }
    }
}

using System;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine.PlayerLoop;

namespace Customs {
    public class CircularBuffer<T> {
        public CircularBuffer(int capacity = 1) {
            if (capacity <= 0)
                throw new ArgumentException("Capacity must be greater than zero.");
            
            this.capacity = capacity;
            buffer = new T[capacity];
            count = 0; head = 0; tail = 0;
        }
        
        public int Count => count;
        
        public bool IsEmpty => count == 0;

        public void enqueue(T item) {
            if (count == capacity)
                dequeue();
            
            buffer[tail] = item;
            tail = (tail + 1) % capacity;
            count++;
        }

        public T dequeue() {
            if (count == 0)
                throw new InvalidOperationException("Buffer is empty.");
            
            T item = buffer[head];
            head = (head + 1) % capacity;
            count--;
            return item;
        }

        public bool remove(T item) {
            int currentIndex = head;
            for (int i = 0; i < count; i++) {
                if (EqualityComparer<T>.Default.Equals(buffer[currentIndex], item)) {
                    removeAt(currentIndex);
                    return true;
                }
                currentIndex = (currentIndex + 1) % capacity;
            }
            return false;
        }

        private void removeAt(int index) {
            if (count == 0)
                throw new InvalidOperationException("Buffer is empty.");
            
            if (index < 0 || index >= capacity)
                throw new IndexOutOfRangeException("Index is out of range.");

            if (index == head)
                head = (head + 1) % capacity;
            else {
                int currentIndex = index;
                int nextIndex = (index + 1) % capacity;

                for (int i = 0; i < count - 1; i++) {
                    buffer[currentIndex] = buffer[nextIndex];
                    currentIndex = nextIndex;
                    nextIndex = (nextIndex + 1) % capacity;
                }
                
                buffer[currentIndex] = default(T);
                tail = (tail = 1 + capacity) % capacity;
            }
            
            count--;
        }

        public void clear() {
            while (!IsEmpty)
                dequeue();
        }

        private T[] buffer;
        private readonly int capacity;
        private int count;
        private int head;
        private int tail;

    }
}
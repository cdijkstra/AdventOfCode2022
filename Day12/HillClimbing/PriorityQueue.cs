namespace HillClimbing;

class PriorityQueue<T> where T : IComparable<T> {
    private List<T> data;
    private Comparison<T> comparison;
    
    public PriorityQueue(Comparison<T> comparison) {
        this.data = new List<T>();
        this.comparison = comparison;
    }
    
    public void Enqueue(T item) {
        data.Add(item);
        int idx = data.Count - 1;
        while (idx > 0) {
            int parentIdx = (idx - 1) / 2;
            if (comparison(data[idx], data[parentIdx]) >= 0) {
                break;
            }
            T tmp = data[idx];
            data[idx] = data[parentIdx];
            data[parentIdx] = tmp;
            idx = parentIdx;
        }
    }
    
    public T Dequeue() {
        int lastIndex = data.Count - 1;
        T frontItem = data[0];
        data[0] = data[lastIndex];
        data.RemoveAt(lastIndex);
        
        lastIndex--;
        int parentIdx = 0;
        while (true) {
            int childIdx = parentIdx * 2 + 1;
            if (childIdx > lastIndex) {
                break;
            }
            int rightChildIdx = childIdx + 1;
            if (rightChildIdx <= lastIndex && comparison(data[rightChildIdx], data[childIdx]) < 0) {
                childIdx = rightChildIdx;
            }
            if (comparison(data[childIdx], data[parentIdx]) >= 0) {
                break;
            }
            T tmp = data[childIdx];
            data[childIdx] = data[parentIdx];
            data[parentIdx] = tmp;
            parentIdx = childIdx;
        }
        return frontItem;
    }
    
    public int Count => data.Count;
}
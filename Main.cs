class Program
{
    static void Main()
    {
        ArrayProcessor ap = new ArrayProcessor();

        ap.Input();
        ap.Display();

        Console.WriteLine("Sap xep bang Bubble Sort:");
        ap.BubbleSort();
        ap.Display();

        int left = 0, right = 0;
        Console.WriteLine("Sap xe bang Quick Sort: ");
        ap.QuickSort(left,right);
        ap.Display();

        Console.WriteLine("Nhap gia tri can tim: ");
        int key = int.Parse(Console.ReadLine());

        int pos1 = ap.LinearSearch(key);
        Console.WriteLine("Linear Search -> " + (pos1 == -1 ? "Khong tim thay" : $"Tim thay tai vi tri {pos1}"));

        int pos2 = ap.BinarySearch(key);
        Console.WriteLine("Binary Search -> " + (pos2 == -1 ? "Khong tim thay" : $"Tim thay tai vi tri {pos2}"));
    }
}

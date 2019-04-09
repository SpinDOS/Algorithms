using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms
{
    static class Sorts
    {
        public static void InsertionSort(int[] a)
        {
            for (int i = 1; i < a.Length; i++)
            {
                int cur = a[i];
                int j = i;
                for (; j > 0 && a[j - 1] > cur; j--)
                {
                    a[j] = a[j - 1];
                }
                a[j] = cur;
            }
        }

        public static void SelectionSort(int[] a)
        {
            for (int i = 0; i < a.Length - 1; i++)
            {
                int minInd = i;
                for (int j = i + 1; j < a.Length; j++)
                    if (a[j] < a[minInd])
                        minInd = j;
                if (minInd != i)
                    Swap(ref a[minInd], ref a[i]);
            }
        }

        public static void BubbleSort(int[] a)
        {
            bool not_sorted = true;
            while (not_sorted)
            {
                not_sorted = false;
                for (int i = 0; i < a.Length - 1; i++)
                {
                    if (a[i] > a[i + 1])
                    {
                        not_sorted = true;
                        Swap(ref a[i], ref a[i+1]);
                    }
                }
            }
        }

        public static void HeapSort(int[] a)
        {
            MakeHeap(a);
            for (int i = a.Length - 1; i > 0; i--)
                a[i] = RemoveTopItem(a, i + 1);
        }

        private static void MakeHeap(int[] a)
        {
            for (int i = 1; i < a.Length; i++)
            {
                int index = i;
                while (index != 0)
                {
                    int parent = (index - 1)/2;
                    if (a[parent] >= a[index])
                        break;

                    Swap(ref a[parent],ref a[index]);

                    index = parent;
                }
            }
        }

        private static int RemoveTopItem(int[] a, int count)
        {
            int result = a[0];
            a[0] = a[count - 1];
            int index = 0;
            while (true)
            {
                int child1 = 2*index + 1;
                int child2 = 2*index + 2;
                if (child1 >= count || child2 >= count ||
                    (a[index] >= a[child1] && a[index] >= a[child2]))
                    break;

                var swapchild = a[child1] >= a[child2] ? child1 : child2;

                Swap(ref a[index], ref a[swapchild]);

                index = swapchild;
            }
            return result;
        }

        public static void QuickSort(int[] a)
        {
            QuickSort2(a, 0, a.Length - 1);
        }

        private static void QuickSort1(int[] a, int start, int end)
        {
            if (start >= end)
                return;
            int l = start, r = end;
            int x = a[start + DateTime.Now.Ticks % (end - start + 1)];
            while (l <= r)
            {
                while (a[l] < x)
                    l++;
                while (a[r] > x)
                    r--;
                if (l <= r)
                    Swap(ref a[l++], ref a[r--]);
            }
            QuickSort1(a, start, r);
            QuickSort1(a, l, end);
        }

        private static void QuickSort2(int[] a, int start, int end)
        {
            if (start >= end)
                return;
            int l = start, r = end;
            int x_i = start + (int)(DateTime.Now.Ticks % (end - start + 1));
            int x = a[x_i];
            a[x_i] = a[start];
            while (true)
            {
                while (a[r] >= x)
                {
                    r--;
                    if (l >= r)
                        break;
                }
                
                if (l >= r)
                {
                    x_i = l;
                    break;
                }

                a[l++] = a[r];

                while (a[l] < x)
                {
                    l++;
                    if (l >= r)
                        break;
                }
                
                if (l >= r)
                {
                    x_i = r;
                    break;
                }

                a[r--] = a[l];
            }

            a[x_i] = x;
            QuickSort2(a, start, x_i - 1);
            QuickSort2(a, x_i + 1, end);
        }

        private static void Swap(ref int a, ref int b)
        {
            int temp = a;
            a = b;
            b = temp;
        }

        public static void MergeSort(int[] a)
        {
            MergeSort(a, new int[a.Length], 0, a.Length - 1);
        }

        private static void MergeSort(int[] a, int[] temp, int start, int end)
        {
            if (start >= end)
                return;
            int mid = (start + end)/2;
            MergeSort(a, temp, start, mid);
            MergeSort(a, temp, mid + 1, end);

            int l = start;
            int r = mid + 1;
            int s = start;
            while (l <= mid && r <= end)
                temp[s++] = a[l] < a[r] ? a[l++] : a[r++];
            while (l <= mid)
                temp[s++] = a[l++];
            while (r <= end)
                temp[s++] = a[r++];

            for (int i = start; i <= end; i++)
                a[i] = temp[i];
        }

    }
}

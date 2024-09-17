using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Galileo6;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace DataProcessing
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

   
    public partial class MainWindow : Window
    {
        LinkedList<double> linkedListSensorA;
        LinkedList<double> linkedListSensorB;
        

        public MainWindow()
        {
            InitializeComponent();
            linkedListSensorA = new LinkedList<double>(); // Initialize here
            linkedListSensorB = new LinkedList<double>(); // Initialize here
            TextBoxSigma.Text = "10";
            TextBoxMu.Text = "50";

            // Add event handlers for text changes to validate input
            TextBoxSigma.TextChanged += TextBoxSigma_TextChanged;
            TextBoxMu.TextChanged += TextBoxMu_TextChanged;

        }

        private void LoadData()
        {
            var reader = new Galileo6.ReadData();
            linkedListSensorA = new LinkedList<double>();
            linkedListSensorB = new LinkedList<double>();

            double mu = 0.0; // Example value, set according to your requirements
            double sigma = 1.0; // Example value, set according to your requirements

            for (int i = 0; i < 400; i++)
            {
                // Retrieve data from Sensor A and add it to linkedListSensorA
                double sensorAData = reader.SensorA(mu, sigma); // Ensure this method exists in ReadData
                linkedListSensorA.AddLast(sensorAData);

                // Retrieve data from Sensor B and add it to linkedListSensorB
                double sensorBData = reader.SensorB(mu, sigma); // Ensure this method exists in ReadData
                linkedListSensorB.AddLast(sensorBData);
            }
        }

        private void ButtonLoadData_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
            ShowAllSensorData();
            DisplayListboxData(linkedListSensorA, listBoxSensorA);
            DisplayListboxData(linkedListSensorB, listBoxSensorB);
            MessageBox.Show($"Number of nodes in Sensor A LinkedList: {NumberOfNodes(linkedListSensorA)}");
            MessageBox.Show($"Number of nodes in Sensor B LinkedList: {NumberOfNodes(linkedListSensorB)}");

        }

        // Method to display all sensor data in the ListView
        private void ShowAllSensorData()
        {
            ListViewSensorData.Items.Clear();
            var sensorAEnumerator = linkedListSensorA.GetEnumerator();
            var sensorBEnumerator = linkedListSensorB.GetEnumerator();

            while (sensorAEnumerator.MoveNext() && sensorBEnumerator.MoveNext())
            {
                ListViewSensorData.Items.Add(new { SensorA = sensorAEnumerator.Current, SensorB = sensorBEnumerator.Current });
            }
        }

        // Method to count the number of nodes in a LinkedList
        private static int NumberOfNodes(LinkedList<double> linkedList)
        {
            // Return the count of elements in the LinkedList
            return linkedList.Count;
        }

        // Method to display the content of a LinkedList inside a ListBox
        private static void DisplayListboxData(LinkedList<double> linkedList, ListBox listBox)
        {
            // Clear any existing items in the ListBox
            listBox.Items.Clear();

            // Iterate through the LinkedList and add each item to the ListBox
            foreach (var item in linkedList)
            {
                listBox.Items.Add(item);
            }
        }
        private static bool SelectionSort(LinkedList<double> linkedList, out long elapsedTicks)
        {
            if (linkedList == null || linkedList.Count <= 1)
            {
                elapsedTicks = 0;
                return false; // No sorting needed for null or single-element lists
            }

            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            LinkedListNode<double> currentNode = linkedList.First;

            while (currentNode != null)
            {
                LinkedListNode<double> minNode = currentNode;
                LinkedListNode<double> searchNode = currentNode.Next;

                // Find the minimum node in the remaining unsorted part
                while (searchNode != null)
                {
                    if (searchNode.Value < minNode.Value)
                    {
                        minNode = searchNode;
                    }
                    searchNode = searchNode.Next;
                }

                // Swap the values of the current node and the minimum node using tuple
                if (minNode != currentNode)
                {
                    (currentNode.Value, minNode.Value) = (minNode.Value, currentNode.Value);
                }

                // Move to the next node
                currentNode = currentNode.Next;
            }

            stopwatch.Stop();
            elapsedTicks = stopwatch.ElapsedTicks;
            return true;
        }
        private static bool InsertionSort(LinkedList<double> linkedList, out long elapsedTicks)
        {
            if (linkedList == null || linkedList.Count <= 1)
            {
                elapsedTicks = 0;
                return false; // No sorting needed for null or single-element lists
            }

            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            LinkedListNode<double> currentNode = linkedList.First.Next;

            while (currentNode != null)
            {
                double currentValue = currentNode.Value;
                LinkedListNode<double> insertionNode = currentNode.Previous;

                // Move insertionNode to the correct position
                while (insertionNode != null && insertionNode.Value > currentValue)
                {
                    insertionNode = insertionNode.Previous;
                }

                // Remove the current node and reinsert it at the correct position
                LinkedListNode<double> nodeToInsert = currentNode;
                currentNode = currentNode.Next; // Move to the next node before removing currentNode
                linkedList.Remove(nodeToInsert);

                if (insertionNode == null)
                {
                    // Insert at the beginning
                    linkedList.AddFirst(nodeToInsert);
                }
                else
                {
                    // Insert after the insertionNode
                    linkedList.AddAfter(insertionNode, nodeToInsert);
                }
            }

            stopwatch.Stop();
            elapsedTicks = stopwatch.ElapsedTicks;
            return true;
        }

        private void ButtonSelectionSort1_Click(object sender, RoutedEventArgs e)
        {
            long elapsedTicks;
            bool isSorted = SelectionSort(linkedListSensorA, out elapsedTicks);

            TextBoxTimeSortSensorA.Text = elapsedTicks.ToString(); // Display the elapsed time in ticks

            if (isSorted)
            {
                MessageBox.Show("Sensor A data sorted successfully.");
                DisplayListboxData(linkedListSensorA, listBoxSensorA); // Show sorted data in the ListBox
            }
            else
            {
                MessageBox.Show("Sorting failed or not needed.");
            }
        }

        private void ButtonSelectionSort_Click(object sender, RoutedEventArgs e)
        {
            long elapsedTicks;
            bool isSorted = SelectionSort(linkedListSensorB, out elapsedTicks);

            TextBoxTimeSortSensorB.Text = elapsedTicks.ToString(); // Display the elapsed time in ticks

            if (isSorted)
            {
                MessageBox.Show("Sensor B data sorted successfully.");
                DisplayListboxData(linkedListSensorB, listBoxSensorB); // Show sorted data in the ListBox
            }
            else
            {
                MessageBox.Show("Sorting failed or not needed.");
            }
        }

        private void ButtonInsertionSort1_Click(object sender, RoutedEventArgs e)
        {
            long elapsedTicks;
            bool isSorted = InsertionSort(linkedListSensorA, out elapsedTicks);

            TextBoxTimeSortSensorA.Text = elapsedTicks.ToString(); // Display the elapsed time in ticks

            if (isSorted)
            {
                MessageBox.Show("Sensor A data sorted successfully.");
                DisplayListboxData(linkedListSensorA, listBoxSensorA); // Show sorted data in the ListBox
            }
            else
            {
                MessageBox.Show("Sorting failed or not needed.");
            }
        }

        private void ButtonInsertionSort_Click(object sender, RoutedEventArgs e)
        {
            long elapsedTicks;
            bool isSorted = InsertionSort(linkedListSensorB, out elapsedTicks);

            TextBoxTimeSortSensorB.Text = elapsedTicks.ToString(); // Display the elapsed time in ticks

            if (isSorted)
            {
                MessageBox.Show("Sensor B data sorted successfully.");
                DisplayListboxData(linkedListSensorB, listBoxSensorB); // Show sorted data in the ListBox
            }
            else
            {
                MessageBox.Show("Sorting failed or not needed.");
            }
        }
        private static int BinarySearchIterative(LinkedList<double> linkedList, double searchValue, int minimum, int maximum, out long elapsedTicks)
        {
            if (linkedList == null || linkedList.Count == 0 || minimum < 0 || maximum >= linkedList.Count)
            {
                elapsedTicks = 0;
                return -1; // Invalid search parameters
            }

            var stopwatch = Stopwatch.StartNew();

            int left = minimum;
            int right = maximum;

            while (left <= right)
            {
                int mid = left + (right - left) / 2;
                LinkedListNode<double>? midNode = GetNodeAt(linkedList, mid);

                if (midNode == null)
                {
                    elapsedTicks = stopwatch.ElapsedTicks;
                    return -1; // Node at mid index does not exist
                }

                double midValue = midNode.Value;

                if (midValue == searchValue)
                {
                    elapsedTicks = stopwatch.ElapsedTicks;
                    return mid; // Found the search value
                }

                if (midValue < searchValue)
                    left = mid + 1; // Search in the right half
                else
                    right = mid - 1; // Search in the left half
            }

            if (left < linkedList.Count)
            {
                LinkedListNode<double>? leftNode = GetNodeAt(linkedList, left);
                if (leftNode != null)
                {
                    elapsedTicks = stopwatch.ElapsedTicks;
                    return left;
                }
            }

            if (right >= 0)
            {
                LinkedListNode<double>? rightNode = GetNodeAt(linkedList, right);
                if (rightNode != null)
                {
                    elapsedTicks = stopwatch.ElapsedTicks;
                    return right;
                }
            }

            elapsedTicks = stopwatch.ElapsedTicks;
            return -1; // No nearest neighbor found
        }

        // Method for recursive binary search
        private static int BinarySearchRecursive(LinkedList<double> linkedList, double searchValue, int minimum, int maximum, out long elapsedTicks)
        {
            if (linkedList == null || linkedList.Count == 0 || minimum < 0 || maximum >= linkedList.Count)
            {
                elapsedTicks = 0;
                return -1; // Invalid search parameters
            }

            Stopwatch stopwatch = Stopwatch.StartNew();
            int result = BinarySearchRecursiveHelper(linkedList, searchValue, minimum, maximum);
            elapsedTicks = stopwatch.ElapsedTicks;

            return result;
        }

        private static int BinarySearchRecursiveHelper(LinkedList<double> linkedList, double searchValue, int minimum, int maximum)
        {
            if (minimum > maximum)
                return -1; // Base case: search value not found

            int mid = minimum + (maximum - minimum) / 2;
            LinkedListNode<double>? midNode = GetNodeAt(linkedList, mid);

            if (midNode == null)
                return -1; // Node at mid index does not exist

            double midValue = midNode.Value;

            if (midValue == searchValue)
                return mid; // Found the search value

            if (midValue < searchValue)
                return BinarySearchRecursiveHelper(linkedList, searchValue, mid + 1, maximum); // Search in the right half
            else
                return BinarySearchRecursiveHelper(linkedList, searchValue, minimum, mid - 1); // Search in the left half
        }

        // Method to get a node at a specific index
        private static LinkedListNode<double>? GetNodeAt(LinkedList<double> linkedList, int index)
        {
            LinkedListNode<double>? currentNode = linkedList.First;
            for (int i = 0; i < index && currentNode != null; i++)
            {
                currentNode = currentNode.Next;
            }
            return currentNode; // Return node at the specified index
        }


        private void DisplayListboxData(LinkedList<double> linkedList, int index, ListBox listBox)
        {
            if (linkedList == null || index < 0 || index >= linkedList.Count || listBox == null)
                return;

            listBox.Items.Clear(); // Clear existing items

            // Add all items from the linked list to the ListBox
            foreach (var item in linkedList)
            {
                listBox.Items.Add(item);
            }

            // Highlight the target number
            if (index >= 0 && index < linkedList.Count)
            {
                listBox.SelectedItem = linkedList.ElementAt(index); // Select the target item
                listBox.ScrollIntoView(listBox.SelectedItem); // Optionally scroll to the selected item
            }
        }


        private void ButtonBinarySerchIterative1_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(TextBoxSearchValueSensorA.Text, out double searchValue))
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                int resultIndex = BinarySearchIterative(linkedListSensorA, searchValue, 0, linkedListSensorA.Count - 1, out long elapsedTicks);
                stopwatch.Stop();

                TextBoxTimeSearchSensorA.Text = elapsedTicks.ToString(); // Display the elapsed time in ticks

                if (resultIndex != -1)
                {
                    MessageBox.Show($"Value found at index: {resultIndex}", "Search Result", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Value not found", "Search Result", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                DisplayListboxData(linkedListSensorA, resultIndex, listBoxSensorA);
            }
            else
            {
                MessageBox.Show("Please enter a valid number.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonBinarySerchIterative_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(TextBoxSearchValueSensorB.Text, out double searchValue))
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                int resultIndex = BinarySearchIterative(linkedListSensorB, searchValue, 0, linkedListSensorB.Count - 1, out long elapsedTicks);
                stopwatch.Stop();

               TextBoxTimeSearchSensorB.Text = elapsedTicks.ToString(); // Display the elapsed time in ticks

                if (resultIndex != -1)
                {
                    MessageBox.Show($"Value found at index: {resultIndex}", "Search Result", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Value not found", "Search Result", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                DisplayListboxData(linkedListSensorB, resultIndex, listBoxSensorB);
            }
            else
            {
                MessageBox.Show("Please enter a valid number.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void ButtonBinaryRecursive1_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(TextBoxSearchValueSensorA.Text, out double searchValue))
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                int resultIndex = BinarySearchRecursive(linkedListSensorA, searchValue, 0, linkedListSensorB.Count - 1, out long elapsedTicks);
                stopwatch.Stop();

                TextBoxTimeSearchSensorB.Text = elapsedTicks.ToString(); // Display the elapsed time in ticks

                if (resultIndex != -1)
                {
                    MessageBox.Show($"Value found at index: {resultIndex}", "Search Result", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Value not found", "Search Result", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                DisplayListboxData(linkedListSensorA, resultIndex, listBoxSensorA);
            }
            else
            {
                MessageBox.Show("Please enter a valid number.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonBinaryRecursive_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(TextBoxSearchValueSensorB.Text, out double searchValue))
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                int resultIndex = BinarySearchRecursive(linkedListSensorB, searchValue, 0, linkedListSensorB.Count - 1, out long elapsedTicks);
                stopwatch.Stop();

                TextBoxTimeSearchSensorB.Text = elapsedTicks.ToString(); // Display the elapsed time in ticks

                if (resultIndex != -1)
                {
                    MessageBox.Show($"Value found at index: {resultIndex}", "Search Result", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Value not found", "Search Result", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                DisplayListboxData(linkedListSensorB, resultIndex, listBoxSensorB);
            }
            else
            {
                MessageBox.Show("Please enter a valid number.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TextBoxSigma_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (double.TryParse(TextBoxSigma.Text, out double sigmaValue))
            {
                if (sigmaValue < 10 || sigmaValue > 20)
                {
                    TextBlockSigmaError.Text = "Sigma must be between 10 and 20.";
                }
                else
                {
                    TextBlockSigmaError.Text = "";
                }
            }
            else
            {
                TextBlockSigmaError.Text = "Invalid input for Sigma.";
            }
        }

        private void TextBoxMu_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (double.TryParse(TextBoxMu.Text, out double muValue))
            {
                if (muValue < 35 || muValue > 75)
                {
                    TextBlockMuError.Text = "Mu must be between 35 and 75.";
                }
                else
                {
                    TextBlockMuError.Text = "";
                }
            }
            else
            {
                TextBlockMuError.Text = "Invalid input for Mu.";
            }
        }

    }

}
using System;
using System.Collections.Generic;
using System.ComponentModel;

public class SortableBindingList<T> : BindingList<T>
{
    private bool isSortedValue;
    private List<(PropertyDescriptor Property, ListSortDirection Direction)> sortProperties;

    protected override bool SupportsSortingCore => true;

    public void ApplySort(List<(PropertyDescriptor Property, ListSortDirection Direction)> sortProperties)
    {
        this.sortProperties = sortProperties;

        var list = Items as List<T>;
        if (list == null)
            return;

        list.Sort(Compare);

        isSortedValue = true;
        OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
    }

    // For backward compatibility with single-property sorting
    public void ApplySort(PropertyDescriptor property, ListSortDirection direction)
    {
        ApplySort(new List<(PropertyDescriptor, ListSortDirection)> { (property, direction) });
    }

    private int Compare(T x, T y)
    {
        foreach (var sortProperty in sortProperties)
        {
            var prop = sortProperty.Property;
            var direction = sortProperty.Direction;

            var xValue = prop.GetValue(x);
            var yValue = prop.GetValue(y);

            int result;

            if (xValue == null)
                result = yValue == null ? 0 : -1;
            else if (yValue == null)
                result = 1;
            else if (xValue is IComparable comparable)
                result = comparable.CompareTo(yValue);
            else
                result = xValue.Equals(yValue) ? 0 : xValue.ToString().CompareTo(yValue.ToString());

            if (direction == ListSortDirection.Descending)
                result = -result;

            if (result != 0)
                return result;
        }

        return 0;
    }

    protected override void RemoveSortCore()
    {
        isSortedValue = false;
    }

    protected override bool IsSortedCore => isSortedValue;
}

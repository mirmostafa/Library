using System.Windows.Controls.Primitives;

namespace Library.Wpf.Helpers;

public static class UiUtility
{
    /// <summary>
    ///     Selects an item in a hierarchial ItemsControl using a set of options
    /// </summary>
    /// <typeparam name="T">The type of the items present in the control and in the options</typeparam>
    /// <param name="control">The ItemsControl to select an item in</param>
    /// <param name="info">The options used for the selection process</param>
    public static void SetSelectedItem<T>(ItemsControl control, SetSelectedInfo<T> info)
    {
        Check.IfArgumentNotNull(control);
        Check.IfArgumentNotNull(info?.Items);
        
        var currentItem = info.Items.First();

        if (control.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
        // Compare each item in the container and look for the next item
        // in the chain.
        {
            foreach (var item in control.Items)
            {
                // Convert the item if a conversion method exists. Otherwise
                // just cast the item to the desired type.
                var convertedItem = info.ConvertMethod is not null ? info.ConvertMethod(item) : (T)item;

                // Compare the converted item with the item in the chain
                if (info.CompareMethod is not null && info.CompareMethod(convertedItem, currentItem))
                {
                    var container = (ItemsControl)control.ItemContainerGenerator.ContainerFromItem(item);

                    // Replace with the remaining items in the chain
                    info.Items = info.Items.Skip(1);

                    // If no items are left in the chain, then we're finished
                    if (!info.Items.Any())
                    {
                        // Select the last item
                        if (info.OnSelected is not null)
                        {
                            info.OnSelected(container, info);
                        }
                    }
                    else
                    // Request more items and continue the search
                    if (info.OnNeedMoreItems is not null)
                    {
                        info.OnNeedMoreItems(container, info);
                        SetSelectedItem(container, info);
                    }

                    break;
                }
            }
        }
        else
        {
            // If the item containers haven't been generated yet, attach an event
            // and wait for the status to change.
            EventHandler? selectWhenReadyMethod = null;

            var method = selectWhenReadyMethod;
            selectWhenReadyMethod = (ds, de) =>
            {
                if (control.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
                {
                    // Stop listening for status changes on this container
                    control.ItemContainerGenerator.StatusChanged -= method;

                    // Search the container for the item chain
                    SetSelectedItem(control, info);
                }
            };

            control.ItemContainerGenerator.StatusChanged += selectWhenReadyMethod;
        }
    }
}

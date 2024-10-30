using System.Collections.Specialized;
using System.Windows.Controls;

namespace VonNeumannIncremental.Stages.Stage1;

public partial class Stage1View : UserControl
{
    public Stage1View()
    {
        InitializeComponent();

        var incc = MessageLog.Items as INotifyCollectionChanged;
        incc.CollectionChanged += Incc_CollectionChanged;
    }

    private void Incc_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
            ScrollToLastItem();
    }

    private void ScrollToLastItem()
    {
        if (MessageLog.Items.Count > 0)
        {
            var lastItem = MessageLog.Items[MessageLog.Items.Count - 1];
            MessageLog.ScrollIntoView(lastItem);
        }
    }
}

using Foundation;
using Microsoft.Maui.Platform;
using UIKit;

namespace imago.Utils;

public static class DragDropHelper
{
    public static void RegisterDragDrop(UIView view, Func<string, Task>? content)
    {
        var dropInteraction = new UIDropInteraction(new DropInteractionDelegate()
        {
            Content = content
        });
        view.AddInteraction(dropInteraction);
    }

    public static void UnRegisterDragDrop(UIView view)
    {
        var dropInteractions = view.Interactions.OfType<UIDropInteraction>();
        foreach (var interaction in dropInteractions)
        {
            view.RemoveInteraction(interaction);
        }
    }
}

class DropInteractionDelegate : UIDropInteractionDelegate
{
    public Func<string, Task>? Content { get; init; }

    public override UIDropProposal SessionDidUpdate(UIDropInteraction interaction, IUIDropSession session)
    {
        return new UIDropProposal(UIDropOperation.Copy);
    }

    public override void PerformDrop(UIDropInteraction interaction, IUIDropSession session)
    {
        foreach (var item in session.Items)
        {
            item.ItemProvider.LoadItem(UniformTypeIdentifiers.UTTypes.Image.Identifier, null, async (data, error) =>
            {
                if (data is NSUrl nsData && !string.IsNullOrEmpty(nsData.Path))
                {
                    if (Content is not null)
                    {
                        await Content.Invoke(nsData.Path);
                    }
                }
            });
        }
    }
}

public static class DragDropExtensions
{
    public static void RegisterDrop(this IElement element, IMauiContext? mauiContext, Func<string, Task>? content)
    {
        ArgumentNullException.ThrowIfNull(mauiContext);
        var view = element.ToPlatform(mauiContext);
        DragDropHelper.RegisterDragDrop(view, content);
    }

    public static void UnRegisterDrop(this IElement element, IMauiContext? mauiContext)
    {
        ArgumentNullException.ThrowIfNull(mauiContext);
        var view = element.ToPlatform(mauiContext);
        DragDropHelper.UnRegisterDragDrop(view);
    }
}
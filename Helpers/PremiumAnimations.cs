namespace PassportMD.Helpers;

public static class Anim
{
    public static async Task FadeSlideIn(VisualElement el, uint duration = 400, int delay = 0)
    {
        el.Opacity = 0;
        el.TranslationY = 30;
        if (delay > 0) await Task.Delay(delay);
        await Task.WhenAll(
            el.FadeTo(1, duration, Easing.CubicOut),
            el.TranslateTo(0, 0, duration, Easing.CubicOut)
        );
    }

    public static async Task BouncePress(VisualElement el)
    {
        await el.ScaleTo(0.92, 80, Easing.CubicIn);
        await el.ScaleTo(1.0, 150, Easing.BounceOut);
    }

    public static async Task SuccessPulse(VisualElement el)
    {
        await el.ScaleTo(1.08, 180, Easing.CubicOut);
        await el.ScaleTo(1.0, 220, Easing.BounceOut);
    }

    public static async Task CelebrationBounce(VisualElement el)
    {
        await el.ScaleTo(1.15, 200, Easing.CubicOut);
        await el.ScaleTo(0.95, 100, Easing.CubicIn);
        await el.ScaleTo(1.05, 100, Easing.CubicOut);
        await el.ScaleTo(1.0, 100, Easing.CubicIn);
    }

    public static async Task AttentionPulse(VisualElement el, int count = 2)
    {
        for (int i = 0; i < count; i++)
        {
            await el.ScaleTo(1.04, 350, Easing.SinInOut);
            await el.ScaleTo(1.0, 350, Easing.SinInOut);
        }
    }

    public static async Task StaggerIn(VisualElement[] elements, uint duration = 350, int stagger = 80)
    {
        foreach (var el in elements)
        {
            el.Opacity = 0;
            el.TranslationY = 25;
        }

        var pending = new List<Task>();
        foreach (var el in elements)
        {
            pending.Add(AnimateIn(el, duration));
            await Task.Delay(stagger);
        }
        await Task.WhenAll(pending);
    }

    private static Task AnimateIn(VisualElement el, uint duration)
    {
        return Task.WhenAll(
            el.FadeTo(1, duration, Easing.CubicOut),
            el.TranslateTo(0, 0, duration, Easing.CubicOut)
        );
    }

    public static async Task PageContentIn(VisualElement layout)
    {
        layout.Opacity = 0;
        layout.TranslationY = 40;
        await Task.WhenAll(
            layout.FadeTo(1, 450, Easing.CubicOut),
            layout.TranslateTo(0, 0, 450, Easing.CubicOut)
        );
    }

    public static void EnsureVisible(params VisualElement[] elements)
    {
        foreach (var el in elements)
        {
            el.Opacity = 1;
            el.TranslationY = 0;
            el.Scale = 1;
        }
    }
}

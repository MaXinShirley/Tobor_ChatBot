using System;

public static class Events
{
    public static Action<int> AnimationSelected;
    public static void OnSelectAnimation(int index) => AnimationSelected?.Invoke(index);

    public static Action<int> ExpressionSelected;
    public static void OnSelectExpression(int index) => ExpressionSelected?.Invoke(index);

    public static Action toborSpawned;
    public static void OnToborSpawned() => toborSpawned?.Invoke();
}

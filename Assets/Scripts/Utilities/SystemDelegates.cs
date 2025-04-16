namespace JTUtility
{
    public delegate void Action();

    public delegate void Action<in T1>(T1 t1);

    public delegate void Action<in T1, in T2>(T1 t1, T2 t2);

    public delegate void Action<in T1, in T2, in T3>(T1 t1, T2 t2, T3 t3);

    public delegate void Action<in T1, in T2, in T3, in T4>(T1 t1, T2 t2, T3 t3, T4 t4);

    public delegate TR Func<out TR>();

    public delegate TR Func<in T1, out TR>(T1 t1);

    public delegate TR Func<in T1, in T2, out TR>(T1 t1, T2 t2);

    public delegate TR Func<in T1, in T2, in T3, out TR>(T1 t1, T2 t2, T3 t3);

    public delegate TR Func<in T1, in T2, in T3, in T4, out TR>(T1 t1, T2 t2, T3 t3, T4 t4);
}
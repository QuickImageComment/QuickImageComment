namespace QuickImageComment.Interfaces
{
    // The generic approach in FormCustomization.Customizer to change foreground
    // and background colors of controls may not be sufficient for some forms.
    // This applies especially for forms which generate controls dynamically,
    // such as the FormFind.
    // This interface allows to implement a special theme change for such forms.
    internal interface IThemeChangeSpecial
    {
        void setThemeSpecial();
    }
}

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace IconFontWpf
{
    public abstract class IconFontBase : Control
    {
        internal abstract void UpdateData();
    }

    public abstract class IconFontBase<TKind> : IconFontBase where TKind : Enum
    {
        private static Lazy<IDictionary<TKind, string>> _dataIndex;

        /// <param name="dataIndexFactory">
        /// Inheritors should provide a factory for setting up the path data index (per icon kind).
        /// The factory will only be utilised once, across all closed instances (first instantiation wins).
        /// </param>
        protected IconFontBase(Func<IDictionary<TKind, string>> dataIndexFactory)
        {
            if (dataIndexFactory == null) throw new ArgumentNullException(nameof(dataIndexFactory));

            if (_dataIndex == null)
                _dataIndex = new Lazy<IDictionary<TKind, string>>(dataIndexFactory);
        }

        public static readonly DependencyProperty KindProperty
            = DependencyProperty.Register(nameof(Kind), typeof(TKind), typeof(IconFontBase<TKind>),
                new PropertyMetadata(default(TKind), KindPropertyChangedCallback));

        private static void KindPropertyChangedCallback(DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            ((IconFontBase) dependencyObject).UpdateData();
        }

        /// <summary>
        /// Gets or sets the icon to display.
        /// </summary>
        public TKind Kind
        {
            get => (TKind) GetValue(KindProperty);
            set => SetValue(KindProperty, value);
        }

        private static readonly DependencyPropertyKey DataPropertyKey
            = DependencyProperty.RegisterReadOnly(nameof(Data), typeof(string), typeof(IconFontBase<TKind>),
                new PropertyMetadata(""));

        // ReSharper disable once StaticMemberInGenericType
        public static readonly DependencyProperty DataProperty = DataPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the font code data for the current <see cref="Kind"/>.
        /// </summary>
        public string Data
        {
            get => (string) GetValue(DataProperty);
            private set => SetValue(DataPropertyKey, value);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            UpdateData();
        }

        internal override void UpdateData()
        {
            string data = null;
            _dataIndex.Value?.TryGetValue(Kind, out data);
            Data = data;
        }
    }
}
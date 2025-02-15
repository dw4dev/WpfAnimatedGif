﻿using System;
using System.ComponentModel;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;

using WpfAnimatedGif.Decoding;

namespace WpfAnimatedGif
{
    /// <summary>
    /// Provides attached properties that display animated GIFs in a standard Image control.
    /// </summary>
    public static class ImageBehavior
    {
        #region Public attached properties and events

        /// <summary>
        /// Gets the value of the <c>AnimatedSource</c> attached property for the specified object.
        /// </summary>
        /// <param name="obj">The element from which to read the property value.</param>
        /// <returns>The currently displayed animated image.</returns>
        [AttachedPropertyBrowsableForType(typeof(Image))]
        public static ImageSource GetAnimatedSource(Image obj)
        {
            return (ImageSource)obj.GetValue(AnimatedSourceProperty);
        }

        /// <summary>
        /// Sets the value of the <c>AnimatedSource</c> attached property for the specified object.
        /// </summary>
        /// <param name="obj">The element on which to set the property value.</param>
        /// <param name="value">The animated image to display.</param>
        public static void SetAnimatedSource(Image obj, ImageSource value)
        {
            obj.SetValue(AnimatedSourceProperty, value);
        }

        /// <summary>
        /// Identifies the <c>AnimatedSource</c> attached property.
        /// </summary>
        public static readonly DependencyProperty AnimatedSourceProperty =
            DependencyProperty.RegisterAttached(
              "AnimatedSource",
              typeof(ImageSource),
              typeof(ImageBehavior),
              new PropertyMetadata(
                null,
                AnimatedSourceChanged));

        /// <summary>
        /// Gets the value of the <c>RepeatBehavior</c> attached property for the specified object.
        /// </summary>
        /// <param name="obj">The element from which to read the property value.</param>
        /// <returns>The repeat behavior of the animated image.</returns>
        [AttachedPropertyBrowsableForType(typeof(Image))]
        public static RepeatBehavior GetRepeatBehavior(Image obj)
        {
            return (RepeatBehavior)obj.GetValue(RepeatBehaviorProperty);
        }

        /// <summary>
        /// Sets the value of the <c>RepeatBehavior</c> attached property for the specified object.
        /// </summary>
        /// <param name="obj">The element on which to set the property value.</param>
        /// <param name="value">The repeat behavior of the animated image.</param>
        public static void SetRepeatBehavior(Image obj, RepeatBehavior value)
        {
            obj.SetValue(RepeatBehaviorProperty, value);
        }

        /// <summary>
        /// Identifies the <c>RepeatBehavior</c> attached property.
        /// </summary>
        public static readonly DependencyProperty RepeatBehaviorProperty =
            DependencyProperty.RegisterAttached(
              "RepeatBehavior",
              typeof(RepeatBehavior),
              typeof(ImageBehavior),
              new PropertyMetadata(
                  default(RepeatBehavior),
                  AnimationPropertyChanged));

        /// <summary>
        /// Gets the value of the <c>AnimationSpeedRatio</c> attached property for the specified object.
        /// </summary>
        /// <param name="obj">The element from which to read the property value.</param>
        /// <returns>The speed ratio for the animated image.</returns>
        public static double? GetAnimationSpeedRatio(DependencyObject obj)
        {
            return (double?)obj.GetValue(AnimationSpeedRatioProperty);
        }

        /// <summary>
        /// Sets the value of the <c>AnimationSpeedRatio</c> attached property for the specified object.
        /// </summary>
        /// <param name="obj">The element on which to set the property value.</param>
        /// <param name="value">The speed ratio of the animated image.</param>
        /// <remarks>The <c>AnimationSpeedRatio</c> and <c>AnimationDuration</c> properties are mutually exclusive, only one can be set at a time.</remarks>
        public static void SetAnimationSpeedRatio(DependencyObject obj, double? value)
        {
            obj.SetValue(AnimationSpeedRatioProperty, value);
        }

        /// <summary>
        /// Identifies the <c>AnimationSpeedRatio</c> attached property.
        /// </summary>
        public static readonly DependencyProperty AnimationSpeedRatioProperty =
            DependencyProperty.RegisterAttached(
                "AnimationSpeedRatio",
                typeof(double?),
                typeof(ImageBehavior),
                new PropertyMetadata(
                    null,
                    AnimationPropertyChanged));

        /// <summary>
        /// Gets the value of the <c>AnimationDuration</c> attached property for the specified object.
        /// </summary>
        /// <param name="obj">The element from which to read the property value.</param>
        /// <returns>The duration for the animated image.</returns>
        public static Duration? GetAnimationDuration(DependencyObject obj)
        {
            return (Duration?)obj.GetValue(AnimationDurationProperty);
        }

        /// <summary>
        /// Sets the value of the <c>AnimationDuration</c> attached property for the specified object.
        /// </summary>
        /// <param name="obj">The element on which to set the property value.</param>
        /// <param name="value">The duration of the animated image.</param>
        /// <remarks>The <c>AnimationSpeedRatio</c> and <c>AnimationDuration</c> properties are mutually exclusive, only one can be set at a time.</remarks>
        public static void SetAnimationDuration(DependencyObject obj, Duration? value)
        {
            obj.SetValue(AnimationDurationProperty, value);
        }

        /// <summary>
        /// Identifies the <c>AnimationDuration</c> attached property.
        /// </summary>
        public static readonly DependencyProperty AnimationDurationProperty =
            DependencyProperty.RegisterAttached(
                "AnimationDuration",
                typeof(Duration?),
                typeof(ImageBehavior),
                new PropertyMetadata(
                    null,
                    AnimationPropertyChanged));

        /// <summary>
        /// Gets the value of the <c>AnimateInDesignMode</c> attached property for the specified object.
        /// </summary>
        /// <param name="obj">The element from which to read the property value.</param>
        /// <returns>true if GIF animations are shown in design mode; false otherwise.</returns>
        public static bool GetAnimateInDesignMode(DependencyObject obj)
        {
            return (bool)obj.GetValue(AnimateInDesignModeProperty);
        }

        /// <summary>
        /// Sets the value of the <c>AnimateInDesignMode</c> attached property for the specified object.
        /// </summary>
        /// <param name="obj">The element on which to set the property value.</param>
        /// <param name="value">true to show GIF animations in design mode; false otherwise.</param>
        public static void SetAnimateInDesignMode(DependencyObject obj, bool value)
        {
            obj.SetValue(AnimateInDesignModeProperty, value);
        }

        /// <summary>
        /// Identifies the <c>AnimateInDesignMode</c> attached property.
        /// </summary>
        public static readonly DependencyProperty AnimateInDesignModeProperty =
            DependencyProperty.RegisterAttached(
                "AnimateInDesignMode",
                typeof(bool),
                typeof(ImageBehavior),
                new FrameworkPropertyMetadata(
                    false,
                    FrameworkPropertyMetadataOptions.Inherits,
                    AnimateInDesignModeChanged));

        /// <summary>
        /// Gets the value of the <c>AutoStart</c> attached property for the specified object.
        /// </summary>
        /// <param name="obj">The element from which to read the property value.</param>
        /// <returns>true if the animation should start immediately when loaded. Otherwise, false.</returns>
        [AttachedPropertyBrowsableForType(typeof(Image))]
        public static bool GetAutoStart(Image obj)
        {
            return (bool)obj.GetValue(AutoStartProperty);
        }

        /// <summary>
        /// Sets the value of the <c>AutoStart</c> attached property for the specified object.
        /// </summary>
        /// <param name="obj">The element from which to read the property value.</param>
        /// <param name="value">true if the animation should start immediately when loaded. Otherwise, false.</param>
        /// <remarks>The default value is true.</remarks>
        public static void SetAutoStart(Image obj, bool value)
        {
            obj.SetValue(AutoStartProperty, value);
        }

        /// <summary>
        /// Identifies the <c>AutoStart</c> attached property.
        /// </summary>
        public static readonly DependencyProperty AutoStartProperty =
            DependencyProperty.RegisterAttached("AutoStart", typeof(bool), typeof(ImageBehavior), new PropertyMetadata(true));

        /// <summary>
        /// Gets the animation controller for the specified <c>Image</c> control.
        /// </summary>
        /// <param name="imageControl"></param>
        /// <returns></returns>
        public static ImageAnimationController GetAnimationController(Image imageControl)
        {
            return (ImageAnimationController)imageControl.GetValue(AnimationControllerPropertyKey.DependencyProperty);
        }

        private static void SetAnimationController(DependencyObject obj, ImageAnimationController value)
        {
            obj.SetValue(AnimationControllerPropertyKey, value);
        }

        private static readonly DependencyPropertyKey AnimationControllerPropertyKey =
            DependencyProperty.RegisterAttachedReadOnly("AnimationController", typeof(ImageAnimationController), typeof(ImageBehavior), new PropertyMetadata(null));

        /// <summary>
        /// Gets the value of the <c>IsAnimationLoaded</c> attached property for the specified object.
        /// </summary>
        /// <param name="image">The element from which to read the property value.</param>
        /// <returns>true if the animation is loaded. Otherwise, false.</returns>
        public static bool GetIsAnimationLoaded(Image image)
        {
            return (bool)image.GetValue(IsAnimationLoadedProperty);
        }

        private static void SetIsAnimationLoaded(Image image, bool value)
        {
            image.SetValue(IsAnimationLoadedPropertyKey, value);
        }

        private static readonly DependencyPropertyKey IsAnimationLoadedPropertyKey =
            DependencyProperty.RegisterAttachedReadOnly("IsAnimationLoaded", typeof(bool), typeof(ImageBehavior), new PropertyMetadata(false));

        /// <summary>
        /// Identifies the <c>IsAnimationLoaded</c> attached property.
        /// </summary>
        public static readonly DependencyProperty IsAnimationLoadedProperty =
            IsAnimationLoadedPropertyKey.DependencyProperty;

        /// <summary>
        /// Identifies the <c>AnimationLoaded</c> attached event.
        /// </summary>
        public static readonly RoutedEvent AnimationLoadedEvent =
            EventManager.RegisterRoutedEvent(
                "AnimationLoaded",
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(ImageBehavior));

        /// <summary>
        /// Adds a handler for the AnimationLoaded attached event.
        /// </summary>
        /// <param name="image">The UIElement that listens to this event.</param>
        /// <param name="handler">The event handler to be added.</param>
        public static void AddAnimationLoadedHandler(Image image, RoutedEventHandler handler)
        {
            if (image == null)
                throw new ArgumentNullException("image");
            if (handler == null)
                throw new ArgumentNullException("handler");
            image.AddHandler(AnimationLoadedEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the AnimationLoaded attached event.
        /// </summary>
        /// <param name="image">The UIElement that listens to this event.</param>
        /// <param name="handler">The event handler to be removed.</param>
        public static void RemoveAnimationLoadedHandler(Image image, RoutedEventHandler handler)
        {
            if (image == null)
                throw new ArgumentNullException("image");
            if (handler == null)
                throw new ArgumentNullException("handler");
            image.RemoveHandler(AnimationLoadedEvent, handler);
        }

        /// <summary>
        /// Identifies the <c>AnimationCompleted</c> attached event.
        /// </summary>
        public static readonly RoutedEvent AnimationCompletedEvent =
            EventManager.RegisterRoutedEvent(
                "AnimationCompleted",
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(ImageBehavior));

        /// <summary>
        /// Adds a handler for the AnimationCompleted attached event.
        /// </summary>
        /// <param name="d">The UIElement that listens to this event.</param>
        /// <param name="handler">The event handler to be added.</param>
        public static void AddAnimationCompletedHandler(Image d, RoutedEventHandler handler)
        {
            var element = d as UIElement;
            if (element == null)
                return;
            element.AddHandler(AnimationCompletedEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the AnimationCompleted attached event.
        /// </summary>
        /// <param name="d">The UIElement that listens to this event.</param>
        /// <param name="handler">The event handler to be removed.</param>
        public static void RemoveAnimationCompletedHandler(Image d, RoutedEventHandler handler)
        {
            var element = d as UIElement;
            if (element == null)
                return;
            element.RemoveHandler(AnimationCompletedEvent, handler);
        }

        #endregion

        private static void AnimatedSourceChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            Image imageControl = o as Image;
            if (imageControl == null)
                return;

            var oldValue = e.OldValue as ImageSource;
            var newValue = e.NewValue as ImageSource;
            if (ReferenceEquals(oldValue, newValue))
            {
                if (imageControl.IsLoaded)
                {
                    var isAnimLoaded = GetIsAnimationLoaded(imageControl);
                    if (!isAnimLoaded)
                        InitAnimationOrImage(imageControl);
                }
                return;
            }
            if (oldValue != null)
            {
                imageControl.Loaded -= ImageControlLoaded;
                imageControl.Unloaded -= ImageControlUnloaded;
                imageControl.IsVisibleChanged -= VisibilityChanged;

                AnimationCache.RemoveControlForSource(oldValue, imageControl);
                var controller = GetAnimationController(imageControl);
                if (controller != null)
                    controller.Dispose();
                imageControl.Source = null;
            }
            if (newValue != null)
            {
                imageControl.Loaded += ImageControlLoaded;
                imageControl.Unloaded += ImageControlUnloaded;
                imageControl.IsVisibleChanged += VisibilityChanged;

                if (imageControl.IsLoaded)
                    InitAnimationOrImage(imageControl);
            }
        }

        private static void VisibilityChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is Image img && img.IsLoaded)
            {
                var controller = GetAnimationController(img);
                if (controller != null)
                {
                    bool isVisible = (bool)e.NewValue;
                    controller.SetSuspended(!isVisible);
                }
            }
        }

        private static void ImageControlLoaded(object sender, RoutedEventArgs e)
        {
            Image imageControl = sender as Image;
            if (imageControl == null)
                return;
            InitAnimationOrImage(imageControl);
        }

        static void ImageControlUnloaded(object sender, RoutedEventArgs e)
        {
            Image imageControl = sender as Image;
            if (imageControl == null)
                return;
            var source = GetAnimatedSource(imageControl);
            if (source != null)
                AnimationCache.RemoveControlForSource(source, imageControl);
            var controller = GetAnimationController(imageControl);
            if (controller != null)
                controller.Dispose();
        }

        private static void AnimationPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            Image imageControl = o as Image;
            if (imageControl == null)
                return;

            ImageSource source = GetAnimatedSource(imageControl);
            if (source != null)
            {
                if (imageControl.IsLoaded)
                    InitAnimationOrImage(imageControl);
            }
        }

        private static void AnimateInDesignModeChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            Image imageControl = o as Image;
            if (imageControl == null)
                return;

            bool newValue = (bool)e.NewValue;

            ImageSource source = GetAnimatedSource(imageControl);
            if (source != null && imageControl.IsLoaded)
            {
                if (newValue)
                    InitAnimationOrImage(imageControl);
                else
                    imageControl.BeginAnimation(Image.SourceProperty, null);
            }
        }

        private static void InitAnimationOrImage(Image imageControl)
        {
            var controller = GetAnimationController(imageControl);
            if (controller != null)
                controller.Dispose();
            SetAnimationController(imageControl, null);
            SetIsAnimationLoaded(imageControl, false);

            var rawSource = GetAnimatedSource(imageControl);
            BitmapSource source = rawSource as BitmapSource;
            if (source == null && rawSource != null)
            {
                imageControl.Source = rawSource;
                return;
            }

            bool isInDesignMode = DesignerProperties.GetIsInDesignMode(imageControl);
            bool animateInDesignMode = GetAnimateInDesignMode(imageControl);
            bool shouldAnimate = !isInDesignMode || animateInDesignMode;

            // For a BitmapImage with a relative UriSource, the loading is deferred until
            // BaseUri is set. This method will be called again when BaseUri is set.
            bool isLoadingDeferred = IsLoadingDeferred(source, imageControl);

            if (source != null && shouldAnimate && !isLoadingDeferred)
            {
                // Case of image being downloaded: retry after download is complete
                if (source.IsDownloading)
                {
                    EventHandler handler = null;
                    handler = (sender, args) =>
                    {
                        source.DownloadCompleted -= handler;
                        InitAnimationOrImage(imageControl);
                    };
                    source.DownloadCompleted += handler;
                    imageControl.Source = source;
                    return;
                }

                /*
                    從快取中取的解碼後的動畫物件，
                    若快取沒有則進行 GIF 物件解碼，然後存到快取中。
                    裡面包含GIF各幀已經渲染好的完整影像。
                */
                var animation = GetAnimation(imageControl, source);
                if (animation != null)
                {
                    if (animation.KeyFrames.Count > 0)
                    {
                        // For some reason, it sometimes throws an exception the first time... the second time it works.
                        TryTwice(() => imageControl.Source = (ImageSource)animation.KeyFrames[0].Value);
                    }
                    else
                    {
                        imageControl.Source = source;
                    }

                    /*
                        ImageAnimationController 就是動畫播放控制器
                    */
                    controller = new ImageAnimationController(imageControl, animation, GetAutoStart(imageControl));
                    SetAnimationController(imageControl, controller);
                    SetIsAnimationLoaded(imageControl, true);
                    imageControl.RaiseEvent(new RoutedEventArgs(AnimationLoadedEvent, imageControl));
                    return;
                }
            }
            imageControl.Source = source;
            if (source != null)
            {
                SetIsAnimationLoaded(imageControl, true);
                imageControl.RaiseEvent(new RoutedEventArgs(AnimationLoadedEvent, imageControl));
            }
        }

        private static ObjectAnimationUsingKeyFrames GetAnimation(Image imageControl, BitmapSource source)
        {
            var cacheEntry = AnimationCache.Get(source);
            if (cacheEntry == null)
            {
                var decoder = GetDecoder(source, imageControl, out GifFile gifMetadata) as GifBitmapDecoder;
                if (decoder != null && decoder.Frames.Count > 1)
                {
                    var fullSize = GetFullSize(decoder, gifMetadata);
                    int index = 0;
                    var keyFrames = new ObjectKeyFrameCollection();
                    var totalDuration = TimeSpan.Zero;
                    BitmapSource baseFrame = null;
                    foreach (var rawFrame in decoder.Frames)
                    {
                        var metadata = GetFrameMetadata(decoder, gifMetadata, index);

                        var frame = MakeFrame(fullSize, rawFrame, metadata, baseFrame);
                        var keyFrame = new DiscreteObjectKeyFrame(frame, totalDuration);
                        keyFrames.Add(keyFrame);

                        totalDuration += metadata.Delay;

                        switch (metadata.DisposalMethod)
                        {
                            case FrameDisposalMethod.None:
                            case FrameDisposalMethod.DoNotDispose:
                                baseFrame = frame;
                                break;
                            case FrameDisposalMethod.RestoreBackground:
                                if (IsFullFrame(metadata, fullSize))
                                {
                                    baseFrame = null;
                                }
                                else
                                {
                                    baseFrame = ClearArea(frame, metadata);
                                }
                                break;
                            case FrameDisposalMethod.RestorePrevious:
                                // Reuse same base frame
                                break;
                        }

                        index++;
                    }

                    int repeatCount = GetRepeatCountFromMetadata(decoder, gifMetadata);
                    cacheEntry = new AnimationCacheEntry(keyFrames, totalDuration, repeatCount);
                    AnimationCache.Add(source, cacheEntry);
                }
            }

            if (cacheEntry != null)
            {
                var animation = new ObjectAnimationUsingKeyFrames
                {
                    KeyFrames = cacheEntry.KeyFrames,
                    Duration = cacheEntry.Duration,
                    RepeatBehavior = GetActualRepeatBehavior(imageControl, cacheEntry.RepeatCountFromMetadata),
                    SpeedRatio = GetActualSpeedRatio(imageControl, cacheEntry.Duration)
                };

                AnimationCache.AddControlForSource(source, imageControl);
                return animation;
            }

            return null;
        }

        private static double GetActualSpeedRatio(Image imageControl, Duration naturalDuration)
        {
            var speedRatio = GetAnimationSpeedRatio(imageControl);
            var duration = GetAnimationDuration(imageControl);

            if (speedRatio.HasValue && duration.HasValue)
                throw new InvalidOperationException("Cannot set both AnimationSpeedRatio and AnimationDuration");

            if (speedRatio.HasValue)
                return speedRatio.Value;

            if (duration.HasValue)
            {
                if (!duration.Value.HasTimeSpan)
                    throw new InvalidOperationException("AnimationDuration cannot be Automatic or Forever");
                if (duration.Value.TimeSpan.Ticks <= 0)
                    throw new InvalidOperationException("AnimationDuration must be strictly positive");
                return naturalDuration.TimeSpan.Ticks / (double)duration.Value.TimeSpan.Ticks;
            }

            return 1.0;
        }

        private static BitmapSource ClearArea(BitmapSource frame, FrameMetadata metadata)
        {
            DrawingVisual visual = new DrawingVisual();
            using (var context = visual.RenderOpen())
            {
                var fullRect = new Rect(0, 0, frame.PixelWidth, frame.PixelHeight);
                var clearRect = new Rect(metadata.Left, metadata.Top, metadata.Width, metadata.Height);
                var clip = Geometry.Combine(
                    new RectangleGeometry(fullRect),
                    new RectangleGeometry(clearRect),
                    GeometryCombineMode.Exclude,
                    null);
                context.PushClip(clip);
                context.DrawImage(frame, fullRect);
            }

            var bitmap = new RenderTargetBitmap(
                    frame.PixelWidth, frame.PixelHeight,
                    frame.DpiX, frame.DpiY,
                    PixelFormats.Pbgra32);
            bitmap.Render(visual);

            var result = new WriteableBitmap(bitmap);

            if (result.CanFreeze && !result.IsFrozen)
                result.Freeze();
            return result;
        }

        private static void TryTwice(Action action)
        {
            try
            {
                action();
            }
            catch (Exception)
            {
                action();
            }
        }

        private static bool IsLoadingDeferred(BitmapSource source, Image imageControl)
        {
            var bmp = source as BitmapImage;
            if (bmp == null)
                return false;
            if (bmp.UriSource != null && !bmp.UriSource.IsAbsoluteUri)
                return bmp.BaseUri == null && (imageControl as IUriContext)?.BaseUri == null;
            return false;
        }

        private static BitmapDecoder GetDecoder(BitmapSource image, Image imageControl, out GifFile gifFile)
        {
            gifFile = null;
            BitmapDecoder decoder = null;
            Stream stream = null;
            Uri uri = null;
            BitmapCreateOptions createOptions = BitmapCreateOptions.None;

            var bmp = image as BitmapImage;
            if (bmp != null)
            {
                createOptions = bmp.CreateOptions;
                if (bmp.StreamSource != null)
                {
                    stream = bmp.StreamSource;
                }
                else if (bmp.UriSource != null)
                {
                    uri = bmp.UriSource;
                    if (!uri.IsAbsoluteUri)
                    {
                        var baseUri = bmp.BaseUri ?? (imageControl as IUriContext)?.BaseUri;
                        if (baseUri != null)
                            uri = new Uri(baseUri, uri);
                    }
                }
            }
            else
            {
                BitmapFrame frame = image as BitmapFrame;
                if (frame != null)
                {
                    decoder = frame.Decoder;
                    Uri.TryCreate(frame.BaseUri, frame.ToString(), out uri);
                }
            }

            if (decoder == null)
            {
                if (stream != null)
                {
                    stream.Position = 0;
                    decoder = BitmapDecoder.Create(stream, createOptions, BitmapCacheOption.OnLoad);
                }
                else if (uri != null && uri.IsAbsoluteUri)
                {
                    decoder = BitmapDecoder.Create(uri, createOptions, BitmapCacheOption.OnLoad);
                }
            }

            if (decoder is GifBitmapDecoder && !CanReadNativeMetadata(decoder))
            {
                if (stream != null)
                {
                    stream.Position = 0;
                    gifFile = GifFile.ReadGifFile(stream, true);
                }
                else if (uri != null)
                {
                    gifFile = DecodeGifFile(uri);
                }
                else
                {
                    throw new InvalidOperationException("Can't get URI or Stream from the source. AnimatedSource should be either a BitmapImage, or a BitmapFrame constructed from a URI.");
                }
            }
            if (decoder == null)
            {
                throw new InvalidOperationException("Can't get a decoder from the source. AnimatedSource should be either a BitmapImage or a BitmapFrame.");
            }
            return decoder;
        }

        private static bool CanReadNativeMetadata(BitmapDecoder decoder)
        {
            try
            {
                var m = decoder.Metadata;
                return m != null;
            }
            catch
            {
                return false;
            }
        }

        private static GifFile DecodeGifFile(Uri uri)
        {
            Stream stream = null;
            if (uri.Scheme == PackUriHelper.UriSchemePack)
            {
                StreamResourceInfo sri;
                if (uri.Authority == "siteoforigin:,,,")
                    sri = Application.GetRemoteStream(uri);
                else
                    sri = Application.GetResourceStream(uri);

                if (sri != null)
                    stream = sri.Stream;
            }
            else
            {
                WebClient wc = new WebClient();
                stream = wc.OpenRead(uri);
            }
            if (stream != null)
            {
                using (stream)
                {
                    return GifFile.ReadGifFile(stream, true);
                }
            }
            return null;
        }

        private static bool IsFullFrame(FrameMetadata metadata, Int32Size fullSize)
        {
            return metadata.Left == 0
                   && metadata.Top == 0
                   && metadata.Width == fullSize.Width
                   && metadata.Height == fullSize.Height;
        }

        private static BitmapSource MakeFrame(
            Int32Size fullSize,
            BitmapSource rawFrame, FrameMetadata metadata,
            BitmapSource baseFrame)
        {
            if (baseFrame == null && IsFullFrame(metadata, fullSize))
            {
                // No previous image to combine with, and same size as the full image
                // Just return the frame as is
                return rawFrame;
            }

            DrawingVisual visual = new DrawingVisual();
            using (var context = visual.RenderOpen())
            {
                if (baseFrame != null)
                {
                    var fullRect = new Rect(0, 0, fullSize.Width, fullSize.Height);
                    context.DrawImage(baseFrame, fullRect);
                }

                var rect = new Rect(metadata.Left, metadata.Top, metadata.Width, metadata.Height);
                context.DrawImage(rawFrame, rect);
            }
            var bitmap = new RenderTargetBitmap(
                fullSize.Width, fullSize.Height,
                96, 96,
                PixelFormats.Pbgra32);
            bitmap.Render(visual);

            var result = new WriteableBitmap(bitmap);

            if (result.CanFreeze && !result.IsFrozen)
                result.Freeze();
            return result;
        }

        private static RepeatBehavior GetActualRepeatBehavior(Image imageControl, int repeatCountFromMetadata)
        {
            // If specified explicitly, use this value
            var repeatBehavior = GetRepeatBehavior(imageControl);
            if (repeatBehavior != default(RepeatBehavior))
                return repeatBehavior;

            if (repeatCountFromMetadata == 0)
                return RepeatBehavior.Forever;
            return new RepeatBehavior(repeatCountFromMetadata);
        }

        private static int GetRepeatCountFromMetadata(BitmapDecoder decoder, GifFile gifMetadata)
        {
            if (gifMetadata != null)
            {
                return gifMetadata.RepeatCount;
            }
            else
            {
                var ext = GetApplicationExtension(decoder, "NETSCAPE2.0");
                if (ext != null)
                {
                    byte[] bytes = ext.GetQueryOrNull<byte[]>("/Data");
                    if (bytes != null && bytes.Length >= 4)
                        return BitConverter.ToUInt16(bytes, 2);
                }
                return 1;
            }
        }

        private static BitmapMetadata GetApplicationExtension(BitmapDecoder decoder, string application)
        {
            int count = 0;
            string query = "/appext";
            BitmapMetadata extension = decoder.Metadata.GetQueryOrNull<BitmapMetadata>(query);
            while (extension != null)
            {
                byte[] bytes = extension.GetQueryOrNull<byte[]>("/Application");
                if (bytes != null)
                {
                    string extApplication = Encoding.ASCII.GetString(bytes);
                    if (extApplication == application)
                        return extension;
                }
                query = string.Format("/[{0}]appext", ++count);
                extension = decoder.Metadata.GetQueryOrNull<BitmapMetadata>(query);
            }
            return null;
        }

        private static FrameMetadata GetFrameMetadata(BitmapDecoder decoder, GifFile gifMetadata, int frameIndex)
        {
            if (gifMetadata != null && gifMetadata.Frames.Count > frameIndex)
            {
                return GetFrameMetadata(gifMetadata.Frames[frameIndex]);
            }

            return GetFrameMetadata(decoder.Frames[frameIndex]);
        }

        private static FrameMetadata GetFrameMetadata(BitmapFrame frame)
        {
            var metadata = (BitmapMetadata)frame.Metadata;
            var delay = TimeSpan.FromMilliseconds(100);
            var metadataDelay = metadata.GetQueryOrDefault("/grctlext/Delay", 10);
            if (metadataDelay != 0)
                delay = TimeSpan.FromMilliseconds(metadataDelay * 10);
            var disposalMethod = (FrameDisposalMethod)metadata.GetQueryOrDefault("/grctlext/Disposal", 0);
            var frameMetadata = new FrameMetadata
            {
                Left = metadata.GetQueryOrDefault("/imgdesc/Left", 0),
                Top = metadata.GetQueryOrDefault("/imgdesc/Top", 0),
                Width = metadata.GetQueryOrDefault("/imgdesc/Width", frame.PixelWidth),
                Height = metadata.GetQueryOrDefault("/imgdesc/Height", frame.PixelHeight),
                Delay = delay,
                DisposalMethod = disposalMethod
            };
            return frameMetadata;
        }

        private static FrameMetadata GetFrameMetadata(GifFrame gifMetadata)
        {
            var d = gifMetadata.Descriptor;
            var frameMetadata = new FrameMetadata
            {
                Left = d.Left,
                Top = d.Top,
                Width = d.Width,
                Height = d.Height,
                Delay = TimeSpan.FromMilliseconds(100),
                DisposalMethod = FrameDisposalMethod.None
            };

            var gce = gifMetadata.Extensions.OfType<GifGraphicControlExtension>().FirstOrDefault();
            if (gce != null)
            {
                if (gce.Delay != 0)
                    frameMetadata.Delay = TimeSpan.FromMilliseconds(gce.Delay);
                frameMetadata.DisposalMethod = (FrameDisposalMethod)gce.DisposalMethod;
            }
            return frameMetadata;
        }

        private static Int32Size GetFullSize(BitmapDecoder decoder, GifFile gifMetadata)
        {
            if (gifMetadata != null)
            {
                var lsd = gifMetadata.Header.LogicalScreenDescriptor;
                return new Int32Size(lsd.Width, lsd.Height);
            }
            int width = decoder.Metadata.GetQueryOrDefault("/logscrdesc/Width", 0);
            int height = decoder.Metadata.GetQueryOrDefault("/logscrdesc/Height", 0);
            return new Int32Size(width, height);
        }

        private struct Int32Size
        {
            public Int32Size(int width, int height) : this()
            {
                Width = width;
                Height = height;
            }

            public int Width { get; private set; }
            public int Height { get; private set; }
        }

        private class FrameMetadata
        {
            public int Left { get; set; }
            public int Top { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
            public TimeSpan Delay { get; set; }
            public FrameDisposalMethod DisposalMethod { get; set; }
        }

        private enum FrameDisposalMethod
        {
            None = 0,
            DoNotDispose = 1,
            RestoreBackground = 2,
            RestorePrevious = 3
        }

        private static T GetQueryOrDefault<T>(this BitmapMetadata metadata, string query, T defaultValue)
        {
            if (metadata.ContainsQuery(query))
                return (T)Convert.ChangeType(metadata.GetQuery(query), typeof(T));
            return defaultValue;
        }

        private static T GetQueryOrNull<T>(this BitmapMetadata metadata, string query)
            where T : class
        {
            if (metadata.ContainsQuery(query))
                return metadata.GetQuery(query) as T;
            return null;
        }

        // For debug purposes
        //private static void Save(BitmapSource image, string path)
        //{
        //    var encoder = new PngBitmapEncoder();
        //    encoder.Frames.Add(BitmapFrame.Create(image));
        //    using (var stream = File.OpenWrite(path))
        //    {
        //        encoder.Save(stream);
        //    }
        //}
    }
}

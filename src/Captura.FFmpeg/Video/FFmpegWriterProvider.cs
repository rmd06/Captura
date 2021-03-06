﻿using System.Collections;
using System.Collections.Generic;
using Captura.FFmpeg;

namespace Captura.Models
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class FFmpegWriterProvider : IVideoWriterProvider
    {
        public string Name => "FFmpeg";

        readonly FFmpegSettings _settings;

        public FFmpegWriterProvider(FFmpegSettings Settings)
        {
            _settings = Settings;
        }

        public IEnumerator<IVideoWriterItem> GetEnumerator()
        {
            yield return new X264VideoCodec();
            yield return new XvidVideoCodec();

            // Gif
            yield return new FFmpegGifItem();

            // Hardware
            yield return new QsvHevcVideoCodec();
            yield return NvencVideoCodec.CreateH264();
            yield return NvencVideoCodec.CreateHevc();

            // Post-processing
            yield return new Vp8VideoCodec();
            yield return new Vp9VideoCodec();

            // Custom
            foreach (var item in _settings.CustomCodecs)
            {
                yield return new CustomFFmpegVideoCodec(item);
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public override string ToString() => Name;

        public string Description => @"Use FFmpeg for encoding.
Requires ffmpeg.exe, if not found option for downloading or specifying path is shown.";
    }
}
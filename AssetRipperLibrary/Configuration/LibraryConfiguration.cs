﻿using AssetRipper.Core.Configuration;

namespace AssetRipper.Library.Configuration
{
	public class LibraryConfiguration : CoreConfiguration
	{
		public AudioExportFormat AudioExportFormat { get; set; } = AudioExportFormat.Default;
		public ImageExportFormat ImageExportFormat { get; set; } = ImageExportFormat.Png;
		public ShaderExportMode ShaderExportMode { get; set; } = ShaderExportMode.Dummy;
		public SpriteExportMode SpriteExportMode { get; set; } = SpriteExportMode.Texture2D;
	}
}

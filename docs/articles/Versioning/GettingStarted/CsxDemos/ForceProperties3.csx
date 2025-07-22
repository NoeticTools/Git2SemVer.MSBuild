var context = VersioningContext.Instance!;
context.Logger.LogInfo("Running demo Git2SemVer customisation script.");

context.Outputs.SetAllVersionPropertiesFrom(SemVersion.ParseFrom("1.2.3-Demo.999+ASimpleDemoScriptVersion"));


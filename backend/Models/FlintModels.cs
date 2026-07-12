using System.ComponentModel;
using System.Text.Json.Serialization;

namespace FlintWorkflowBackend.Models;

[Description("The input schema for constructing a Flint chart assembly, describing the dataset, semantic meanings, and charting configurations.")]
public class ChartAssemblyInput
{
    [JsonPropertyName("data")]
    [Description("The data source specification containing raw values or a URL.")]
    public DataSpec Data { get; set; } = new();

    [JsonPropertyName("semantic_types")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [Description("Maps data column names to their semantic types (e.g., 'Category', 'Quantity', 'YearMonth'). Example: { \"Month\": \"YearMonth\", \"Sales\": \"Quantity\" }.")]
    public Dictionary<string, string>? SemanticTypes { get; set; }

    [JsonPropertyName("chart_spec")]
    [Description("The specification defining the chart type, encoding mappings, and layout size.")]
    public ChartSpec ChartSpec { get; set; } = new();

    [JsonPropertyName("options")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [Description("Optional custom engine configuration options (AssembleOptions) for the visual rendering, such as layout tuning or tooltips.")]
    public AssembleOptions? Options { get; set; }

    [JsonPropertyName("field_display_names")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [Description("Localized display names for fields (column name -> display label). When present, used as axis titles and legend headers instead of raw field names.")]
    public Dictionary<string, string>? FieldDisplayNames { get; set; }
}

[Description("Specifies the data values or reference URL for the chart.")]
public class DataSpec
{
    [JsonPropertyName("values")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [Description("A list of data rows represented as dictionaries. Each entry maps a column name to its data point (formatted as a string). Example: [ { \"Month\": \"Jan\", \"Sales\": \"100\" } ].")]
    public List<Dictionary<string, string>>? Values { get; set; }

    [JsonPropertyName("url")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [Description("The URL of an external dataset file, if data values are not embedded directly.")]
    public string? Url { get; set; }
}

[Description("Represents a visual encoding channel mapping. Maps a data field to a visual channel with optional type, aggregation, sorting, and color scheme overrides.")]
public class EncodingChannel
{
    [JsonPropertyName("field")]
    [Description("The name of the data column mapped to this channel.")]
    public string Field { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [Description("The encoding type for this channel. One of: 'quantitative', 'nominal', 'ordinal', 'temporal'. When omitted, the engine infers it from the field's semantic type and data values.")]
    public string? Type { get; set; }

    [JsonPropertyName("aggregate")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [Description("The aggregation function to apply. One of: 'count', 'sum', 'average', 'mean'.")]
    public string? Aggregate { get; set; }

    [JsonPropertyName("sortOrder")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [Description("The sort direction for this channel. One of: 'ascending', 'descending'.")]
    public string? SortOrder { get; set; }

    [JsonPropertyName("sortBy")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [Description("The field name to sort by (when sorting by a different field than the encoded one).")]
    public string? SortBy { get; set; }

    [JsonPropertyName("scheme")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [Description("An explicit color scheme identifier (e.g., 'viridis', 'blues'). Only relevant for color/fill channels.")]
    public string? Scheme { get; set; }
}

[Description("Defines visual parameters, channel encodings, and layout size for the chart.")]
public class ChartSpec
{
    [JsonPropertyName("chartType")]
    [Description("The chart presentation format. (example: Bar Chart, Pie Chart, Line Chart, Scatter Plot)")]
    public string ChartType { get; set; } = string.Empty;

    [JsonPropertyName("encodings")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [Description("Maps visual channels to encoding definitions. Supported channels include: 'x', 'y', 'color', 'size', 'shape', 'column', 'row', 'angle', 'radius', 'opacity', 'group', 'detail', 'order'. Example: { \"x\": { \"field\": \"Month\", \"type\": \"ordinal\" }, \"y\": { \"field\": \"Sales\", \"aggregate\": \"sum\" }, \"color\": { \"field\": \"Region\" } }.")]
    public Dictionary<string, EncodingChannel>? Encodings { get; set; }

    [JsonPropertyName("baseSize")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [Description("The baseline width and height of the visual chart.")]
    public SizeSpec? BaseSize { get; set; }

    [JsonPropertyName("canvasSize")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [Description("The explicit canvas container dimensions for layout scaling.")]
    public SizeSpec? CanvasSize { get; set; }

    [JsonPropertyName("chartProperties")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [Description("Additional presentation properties and layout configurations. Note: Due to strict schema requirements, provide all values as strings (e.g., 'true' for booleans, '4' for numbers).")]
    public Dictionary<string, string>? ChartProperties { get; set; }
}

[Description("Defines dimensions in pixels.")]
public class SizeSpec
{
    [JsonPropertyName("width")]
    [Description("The width dimension value in pixels.")]
    public int Width { get; set; }

    [JsonPropertyName("height")]
    [Description("The height dimension value in pixels.")]
    public int Height { get; set; }
}

[Description("Options for the chart assembler, such as layout tuning and tooltips.")]
public class AssembleOptions
{
    [JsonPropertyName("addTooltips")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [Description("Whether to add tooltips to the chart.")]
    public bool? AddTooltips { get; set; }

    [JsonPropertyName("stepPadding")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [Description("Fraction of each step reserved for inter-category padding (0-1). Default: 0.1.")]
    public double? StepPadding { get; set; }

    [JsonPropertyName("maxStretch")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [Description("Default maximum stretch multiplier used when the spec provides no explicit canvasSize ceiling. Default: 2.")]
    public double? MaxStretch { get; set; }

    [JsonPropertyName("maxStretchX")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [Description("Resolved per-dimension stretch cap for the X (width) axis.")]
    public double? MaxStretchX { get; set; }

    [JsonPropertyName("maxStretchY")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [Description("Resolved per-dimension stretch cap for the Y (height) axis.")]
    public double? MaxStretchY { get; set; }

    [JsonPropertyName("facetElasticity")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [Description("Power-law exponent for facet subplot stretch. Default: 0.3.")]
    public double? FacetElasticity { get; set; }

    [JsonPropertyName("minStep")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [Description("Minimum pixels per discrete axis item. Default: 6.")]
    public double? MinStep { get; set; }

    [JsonPropertyName("maxColorValues")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [Description("Maximum number of distinct color values before overflow truncation. Default: 24.")]
    public int? MaxColorValues { get; set; }

    [JsonPropertyName("minSubplotSize")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [Description("Minimum facet subplot size in px. Default: 60.")]
    public double? MinSubplotSize { get; set; }

    [JsonPropertyName("facetGap")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [Description("Gap in px between adjacent facet panels.")]
    public double? FacetGap { get; set; }

    [JsonPropertyName("defaultBandSize")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [Description("Base pixels per discrete category at a 300px baseline canvas. Default: 20.")]
    public double? DefaultBandSize { get; set; }

    [JsonPropertyName("maintainContinuousAxisRatio")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [Description("When true, continuous X and Y axes stretch together using the larger of the two stretch factors.")]
    public bool? MaintainContinuousAxisRatio { get; set; }

    [JsonPropertyName("facetAspectRatioResistance")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [Description("Resistance to aspect-ratio distortion when faceting. 0 to 1.")]
    public double? FacetAspectRatioResistance { get; set; }

    [JsonPropertyName("autoFacetWrap")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [Description("Whether to auto-wrap column-only facets into a 2D grid.")]
    public bool? AutoFacetWrap { get; set; }

    [JsonPropertyName("targetBandAR")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [Description("Target aspect ratio for a single band (step height ÷ step width).")]
    public double? TargetBandAR { get; set; }
}

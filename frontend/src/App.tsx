import { useEffect, useRef, useState } from 'react';
import { assembleVegaLite, assembleECharts, assembleChartjs } from 'flint-chart';
import embed from 'vega-embed';
import * as echarts from 'echarts';
import Chart from 'chart.js/auto';

// Custom module and component imports
import { PRESETS } from './presets';
import { Header } from './components/Header';
import { Sidebar } from './components/Sidebar';
import { ChartPreview } from './components/ChartPreview';
import { SpecInspector } from './components/SpecInspector';

const mapStrictSpec = (spec: any) => {
  if (!spec) return spec;
  const mapped = { ...spec };
  const toDict = (arr: any[]) => arr?.reduce((acc, curr) => ({ ...acc, [curr.key]: curr.value }), {});

  if (Array.isArray(mapped.semantic_types)) mapped.semantic_types = toDict(mapped.semantic_types);
  if (Array.isArray(mapped.field_display_names)) mapped.field_display_names = toDict(mapped.field_display_names);
  
  if (mapped.chart_spec) {
    if (Array.isArray(mapped.chart_spec.chartProperties)) mapped.chart_spec.chartProperties = toDict(mapped.chart_spec.chartProperties);
    if (mapped.chart_spec.encodings && typeof mapped.chart_spec.encodings === 'object') {
      const encDict: any = {};
      for (const key of Object.keys(mapped.chart_spec.encodings)) {
        if (mapped.chart_spec.encodings[key] !== null && mapped.chart_spec.encodings[key] !== undefined) {
          encDict[key] = mapped.chart_spec.encodings[key];
        }
      }
      mapped.chart_spec.encodings = encDict;
    }
  }
  
  if (mapped.data?.values && Array.isArray(mapped.data.values)) {
    mapped.data.values = mapped.data.values.map((row: any) => 
      row.cells ? toDict(row.cells) : row
    );
  }
  
  return mapped;
};

function App() {
  const [presetKey, setPresetKey] = useState<string>("signups");
  const [editorValue, setEditorValue] = useState<string>("");
  const [activeEngine, setActiveEngine] = useState<"vega" | "echarts" | "chartjs">("vega");
  const [activeInspectorTab, setActiveInspectorTab] = useState<"vega" | "echarts" | "chartjs">("vega");
  const [compileError, setCompileError] = useState<string | null>(null);
  const [copySuccess, setCopySuccess] = useState<boolean>(false);

  const chartContainerRef = useRef<HTMLDivElement | null>(null);
  const echartsInstanceRef = useRef<echarts.ECharts | null>(null);
  const chartjsInstanceRef = useRef<Chart | null>(null);

  // Initialize with default preset
  useEffect(() => {
    setEditorValue(JSON.stringify(PRESETS[presetKey].spec, null, 2));
  }, [presetKey]);

  // Clean up any charts on unmount/re-render
  const cleanupCharts = () => {
    if (echartsInstanceRef.current) {
      echartsInstanceRef.current.dispose();
      echartsInstanceRef.current = null;
    }
    if (chartjsInstanceRef.current) {
      chartjsInstanceRef.current.destroy();
      chartjsInstanceRef.current = null;
    }
    if (chartContainerRef.current) {
      chartContainerRef.current.innerHTML = "";
    }
  };

  // Compile and render the chart
  const renderChart = () => {
    if (!chartContainerRef.current) return;
    cleanupCharts();
    setCompileError(null);

    try {
      const inputSpec = mapStrictSpec(JSON.parse(editorValue));

      if (activeEngine === "vega") {
        const spec = assembleVegaLite(inputSpec);
        embed(chartContainerRef.current, spec, {
          actions: true,
          theme: "dark",
          renderer: "svg"
        }).catch(err => {
          setCompileError(`Vega-Lite Render Error: ${err.message}`);
        });
      } else if (activeEngine === "echarts") {
        const option = assembleECharts(inputSpec);
        option.backgroundColor = "transparent"; // transparent for dark dashboard card overlay
        
        const instance = echarts.init(chartContainerRef.current, "dark");
        echartsInstanceRef.current = instance;
        instance.setOption(option);

        const handleResize = () => instance.resize();
        window.addEventListener("resize", handleResize);
        return () => window.removeEventListener("resize", handleResize);
      } else if (activeEngine === "chartjs") {
        const config = assembleChartjs(inputSpec);
        
        // Custom dark theme styles mapping for Chart.js
        if (config && config.options) {
          if (config.options.scales) {
            Object.keys(config.options.scales).forEach(key => {
              const scale = (config.options.scales as any)[key];
              if (scale) {
                scale.grid = { ...scale.grid, color: "rgba(255, 255, 255, 0.08)" };
                scale.ticks = {
                  ...scale.ticks,
                  color: "#94a3b8",
                  font: { family: "Outfit" }
                };
              }
            });
          }
          if (config.options.plugins && config.options.plugins.legend) {
            if (config.options.plugins.legend.labels) {
              config.options.plugins.legend.labels.color = "#f8fafc";
              config.options.plugins.legend.labels.font = {
                family: "Outfit",
                size: 12
              };
            }
          }
        }

        const canvas = document.createElement("canvas");
        chartContainerRef.current.appendChild(canvas);
        
        const instance = new Chart(canvas, config);
        chartjsInstanceRef.current = instance;
      }
    } catch (err: any) {
      setCompileError(err.message || "Unknown compile/rendering error");
    }
  };

  // Trigger render when spec, engine, or container is ready
  useEffect(() => {
    if (editorValue) {
      renderChart();
    }
    return cleanupCharts;
  }, [editorValue, activeEngine]);

  // Compile helper for inspector tabs
  const getCompiledCodeString = () => {
    try {
      const inputSpec = mapStrictSpec(JSON.parse(editorValue));
      if (activeInspectorTab === "vega") {
        return JSON.stringify(assembleVegaLite(inputSpec), null, 2);
      } else if (activeInspectorTab === "echarts") {
        return JSON.stringify(assembleECharts(inputSpec), null, 2);
      } else if (activeInspectorTab === "chartjs") {
        return JSON.stringify(assembleChartjs(inputSpec), null, 2);
      }
    } catch (err: any) {
      return `Error generating spec: ${err.message}`;
    }
    return "";
  };

  const handleCopy = () => {
    navigator.clipboard.writeText(getCompiledCodeString());
    setCopySuccess(true);
    setTimeout(() => setCopySuccess(false), 2000);
  };

  return (
    <div className="app-container">
      <Header activeEngine={activeEngine} setActiveEngine={setActiveEngine} />
      <main className="main-content">
        <Sidebar
          presetKey={presetKey}
          setPresetKey={setPresetKey}
          editorValue={editorValue}
          setEditorValue={setEditorValue}
          compileError={compileError}
          onCompile={renderChart}
        />
        <section className="preview-area">
          <div className="preview-content">
            <ChartPreview chartContainerRef={chartContainerRef} />
            <SpecInspector
              activeInspectorTab={activeInspectorTab}
              setActiveInspectorTab={setActiveInspectorTab}
              codeString={getCompiledCodeString()}
              onCopy={handleCopy}
              copySuccess={copySuccess}
            />
          </div>
        </section>
      </main>
    </div>
  );
}

export default App;

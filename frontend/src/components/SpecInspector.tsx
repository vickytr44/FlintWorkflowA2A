import { Info, Check, Copy } from "lucide-react";

interface SpecInspectorProps {
  activeInspectorTab: "vega" | "echarts" | "chartjs";
  setActiveInspectorTab: (tab: "vega" | "echarts" | "chartjs") => void;
  codeString: string;
  onCopy: () => void;
  copySuccess: boolean;
}

export function SpecInspector({
  activeInspectorTab,
  setActiveInspectorTab,
  codeString,
  onCopy,
  copySuccess,
}: SpecInspectorProps) {
  return (
    <div className="spec-inspector">
      <div className="tab-nav" style={{ padding: "0 1rem", borderBottom: "1px solid var(--border)" }}>
        <button
          className={`tab-btn ${activeInspectorTab === "vega" ? "active" : ""}`}
          onClick={() => setActiveInspectorTab("vega")}
        >
          Compiled Vega-Lite
        </button>
        <button
          className={`tab-btn ${activeInspectorTab === "echarts" ? "active" : ""}`}
          onClick={() => setActiveInspectorTab("echarts")}
        >
          Compiled ECharts Option
        </button>
        <button
          className={`tab-btn ${activeInspectorTab === "chartjs" ? "active" : ""}`}
          onClick={() => setActiveInspectorTab("chartjs")}
        >
          Compiled Chart.js Config
        </button>
      </div>

      <div className="spec-inspector-header">
        <div className="spec-inspector-title">
          <Info size={12} /> Live Compiled Target Specification
        </div>
        <button className="copy-btn" onClick={onCopy}>
          {copySuccess ? (
            <>
              <Check size={12} style={{ color: "var(--success)" }} /> Copied!
            </>
          ) : (
            <>
              <Copy size={12} /> Copy Code
            </>
          )}
        </button>
      </div>

      <pre className="code-block">
        <code>{codeString}</code>
      </pre>
    </div>
  );
}

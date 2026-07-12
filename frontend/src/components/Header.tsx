interface HeaderProps {
  activeEngine: "vega" | "echarts" | "chartjs";
  setActiveEngine: (engine: "vega" | "echarts" | "chartjs") => void;
}

export function Header({ activeEngine, setActiveEngine }: HeaderProps) {
  return (
    <header className="header">
      <div className="header-title-section">
        <div className="logo-badge">Flint</div>
        <div>
          <h1>Flint Chart Playground</h1>
          <p>A visualization language that maps semantics to stunning charts</p>
        </div>
      </div>
      <div className="engine-badges">
        <button
          className={`engine-badge ${activeEngine === "vega" ? "active" : ""}`}
          onClick={() => setActiveEngine("vega")}
        >
          Vega-Lite
        </button>
        <button
          className={`engine-badge ${activeEngine === "echarts" ? "active" : ""}`}
          onClick={() => setActiveEngine("echarts")}
        >
          ECharts
        </button>
        <button
          className={`engine-badge ${activeEngine === "chartjs" ? "active" : ""}`}
          onClick={() => setActiveEngine("chartjs")}
        >
          Chart.js
        </button>
      </div>
    </header>
  );
}

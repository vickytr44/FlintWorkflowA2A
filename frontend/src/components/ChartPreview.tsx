import React from "react";
import { BarChart2 } from "lucide-react";

interface ChartPreviewProps {
  chartContainerRef: React.RefObject<HTMLDivElement | null>;
}

export function ChartPreview({ chartContainerRef }: ChartPreviewProps) {
  return (
    <div className="visualizer-card">
      <div
        ref={chartContainerRef}
        className="chart-render-target"
        style={{
          width: "100%",
          height: "360px",
          display: "flex",
          alignItems: "center",
          justifyContent: "center",
        }}
      >
        <div className="chart-placeholder">
          <BarChart2 className="chart-placeholder-icon" />
          <p>Chart will render here</p>
        </div>
      </div>
    </div>
  );
}

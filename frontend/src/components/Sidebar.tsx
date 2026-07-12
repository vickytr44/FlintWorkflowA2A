import { Compass, FileText, Play, HelpCircle } from "lucide-react";
import { PRESETS } from "../presets";

interface SidebarProps {
  presetKey: string;
  setPresetKey: (key: string) => void;
  editorValue: string;
  setEditorValue: (value: string) => void;
  compileError: string | null;
  onCompile: () => void;
}

export function Sidebar({
  presetKey,
  setPresetKey,
  editorValue,
  setEditorValue,
  compileError,
  onCompile,
}: SidebarProps) {
  return (
    <section className="sidebar">
      {/* Preset Selector */}
      <div className="form-group">
        <div className="section-title">
          <Compass size={14} /> Spec Presets
        </div>
        <select
          className="select-control"
          value={presetKey}
          onChange={(e) => setPresetKey(e.target.value)}
        >
          {Object.entries(PRESETS).map(([key, val]) => (
            <option key={key} value={key}>
              {val.name}
            </option>
          ))}
        </select>
      </div>

      {/* Code Editor */}
      <div className="editor-container">
        <div className="section-title" style={{ justifyContent: "space-between" }}>
          <span style={{ display: "flex", alignItems: "center", gap: "0.5rem" }}>
            <FileText size={14} /> Flint Input Spec (JSON)
          </span>
          <button
            className="btn-primary"
            style={{ padding: "0.2rem 0.5rem", fontSize: "0.75rem", borderRadius: "4px" }}
            onClick={onCompile}
          >
            <Play size={10} /> Compile
          </button>
        </div>
        <textarea
          className="textarea-control"
          value={editorValue}
          onChange={(e) => setEditorValue(e.target.value)}
          placeholder="Paste Flint Spec JSON here..."
        />
      </div>

      {/* Compilation Error Banner */}
      {compileError && (
        <div className="form-group">
          <div className="section-title" style={{ color: "var(--error)" }}>
            Compilation Error
          </div>
          <pre className="error-banner">{compileError}</pre>
        </div>
      )}

      {/* Quick Help Guide */}
      <div
        className="visualizer-card"
        style={{
          minHeight: "auto",
          padding: "1.25rem",
          gap: "0.75rem",
          background: "rgba(15, 23, 42, 0.4)",
          display: "block",
        }}
      >
        <div className="section-title" style={{ marginBottom: "0.5rem" }}>
          <HelpCircle size={14} /> Quick Guide
        </div>
        <p style={{ fontSize: "0.8rem", color: "var(--text-muted)", lineHeight: "1.5", marginBottom: "0.5rem" }}>
          Flint maps columns to visual aesthetics using two sections:
        </p>
        <ul
          style={{
            paddingLeft: "1.25rem",
            fontSize: "0.8rem",
            color: "var(--text-muted)",
            display: "flex",
            flexDirection: "column",
            gap: "0.4rem",
          }}
        >
          <li>
            <strong style={{ color: "var(--text)" }}>semantic_types</strong>: Assign types like{" "}
            <code style={{ color: "var(--accent)" }}>YearMonth</code>,{" "}
            <code style={{ color: "var(--accent)" }}>Quantity</code>, or{" "}
            <code style={{ color: "var(--accent)" }}>Category</code>.
          </li>
          <li>
            <strong style={{ color: "var(--text)" }}>chart_spec</strong>: Specify template (
            <code style={{ color: "var(--accent)" }}>Line Chart</code>,{" "}
            <code style={{ color: "var(--accent)" }}>Bar Chart</code>) and mappings.
          </li>
        </ul>
      </div>
    </section>
  );
}

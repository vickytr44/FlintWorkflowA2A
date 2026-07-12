export interface Preset {
  name: string;
  spec: any;
}

export const PRESETS: Record<string, Preset> = {
  signups: {
    name: "Monthly Signups (Getting Started)",
    spec: {
      data: {
        values: [
          { month: "2024-01", signups: 120 },
          { month: "2024-02", signups: 146 },
          { month: "2024-03", signups: 168 },
          { month: "2024-04", signups: 164 },
          { month: "2024-05", signups: 181 }
        ]
      },
      semantic_types: {
        month: "YearMonth",
        signups: "Quantity"
      },
      chart_spec: {
        chartType: "Line Chart",
        encodings: {
          x: { field: "month" },
          y: { field: "signups" }
        },
        baseSize: { width: 450, height: 300 }
      }
    }
  },
  revenue: {
    name: "Quarterly Revenue (Bar Chart)",
    spec: {
      data: {
        values: [
          { quarter: "2023-Q1", revenue: 45000 },
          { quarter: "2023-Q2", revenue: 52000 },
          { quarter: "2023-Q3", revenue: 49000 },
          { quarter: "2023-Q4", revenue: 68000 }
        ]
      },
      semantic_types: {
        quarter: "Category",
        revenue: "Quantity"
      },
      chart_spec: {
        chartType: "Bar Chart",
        encodings: {
          x: { field: "quarter" },
          y: { field: "revenue" }
        },
        baseSize: { width: 450, height: 300 }
      }
    }
  },
  marketShare: {
    name: "Market Share (Pie Chart)",
    spec: {
      data: {
        values: [
          { company: "Google", share: 45 },
          { company: "Apple", share: 30 },
          { company: "Microsoft", share: 15 },
          { company: "Others", share: 10 }
        ]
      },
      semantic_types: {
        company: "Category",
        share: "Quantity"
      },
      chart_spec: {
        chartType: "Pie Chart",
        encodings: {
          color: { field: "company" },
          theta: { field: "share" }
        },
        baseSize: { width: 350, height: 350 }
      }
    }
  },
  scatter: {
    name: "Age vs Income (Scatter Plot)",
    spec: {
      data: {
        values: [
          { age: 22, income: 45000 },
          { age: 25, income: 54000 },
          { age: 30, income: 72000 },
          { age: 35, income: 85000 },
          { age: 40, income: 98000 },
          { age: 45, income: 105000 },
          { age: 50, income: 120000 }
        ]
      },
      semantic_types: {
        age: "Quantity",
        income: "Quantity"
      },
      chart_spec: {
        chartType: "Scatter Plot",
        encodings: {
          x: { field: "age" },
          y: { field: "income" }
        },
        baseSize: { width: 450, height: 300 }
      }
    }
  }
};

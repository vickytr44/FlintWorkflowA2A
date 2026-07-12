import { ecAllTemplateDefs } from 'flint-chart/echarts';
import fs from 'fs';

const reference = ecAllTemplateDefs.map(def => ({
    chartType: def.chart,
    description: def.description,
    channels: def.channels
}));

fs.writeFileSync('chart-reference.json', JSON.stringify(reference, null, 2));
console.log('Done!');

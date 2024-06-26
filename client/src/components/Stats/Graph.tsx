import ReactECharts from "echarts-for-react";
import { FC } from "react";
import { useGetActivitesQuery } from "../../services/statsService";

type chartData = {
  leftDate: number,
  rightDate: number,
  [key: string]: any,
}

type props = {
  chartData?: chartData[],
  flag?: boolean,
}

const Graph: FC<props> = ({chartData, flag})  => {
  const { data: activities } = useGetActivitesQuery();
  const options = {
    grid: { top: 18, right: 18, bottom: 24, left: 46 },
    tooltip: {
      trigger: "axis",
    },
    xAxis: {
      type: "category",
      data: chartData?.map((data) => {

        return (
          
          new Date(data.leftDate * 1000).toLocaleDateString() +
          " " +
          new Date(data.leftDate * 1000).toLocaleTimeString("ru-RU", {
            hour: "numeric",
            minute: "numeric",
            hour12: false,
          })
        );
      }),
    },
    yAxis: {
      type: "value",
    },
    series: chartData && (chartData.some(data => 'seconds' in data || 'quantity' in data)) ? [
      {
        name: chartData.some(data => 'seconds' in data) ? 'Минут' : 'Количество',
        data: chartData?.map((data: any) => {
          if ('seconds' in data) {
            return (data.seconds / 60).toFixed(2);
          } 
          return data.quantity;
        }),
        type: "line",
        smooth: true,
      },
    ] : [
      ...(chartData && chartData[0]?.values && Object.keys(chartData[0]?.values).map((item: any) => ({
        name: activities?.filter((element: { id: number; }) => element.id === item)[0]?.title,
        data: chartData?.map((data: any) => {
          const value =  Object.keys(data.values).map((key: any) => {
            if (key === item) {
              return (data.values[key]);
            }
          })
          if (flag) {
            return (value.filter(element => element !== undefined)[0]);
          }
          return (value.filter(element => element !== undefined)[0]  / 60).toFixed(2);
        }),
        type: "line",
        smooth: true,
      })) || [])
    ],
  };

  return (
    <ReactECharts option={options} />
  )
};

export default Graph;

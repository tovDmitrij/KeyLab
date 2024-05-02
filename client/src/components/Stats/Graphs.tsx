import {
  Accordion,
  AccordionDetails,
  AccordionSummary,
  Container,
  Typography,
} from "@mui/material";
import ArrowDropDownIcon from "@mui/icons-material/ArrowDropDown";
import { useAppSelector } from "../../store/redux";
import {
  useAverageTimeMutation,
  useAverageTimeOnPagesMutation,
  useCountUsersMutation,
  useCountUsersOnPagesMutation,
} from "../../services/statsService";
import { useEffect, useState } from "react";
import Graph from "./Graph";

type accordionList = {
  chartData: any;
  title: string; 
  [key: string]: any,
};

const Graphs = () => {
  const { leftDate, rightDate, intervalId, activityIDs } = useAppSelector(
    (state) => state.statsReducer
  );

  const [accordionList, setAccordionList] = useState<accordionList[]>([]);
  const [getAverageTime, { data: averageTime }] = useAverageTimeMutation();
  const [getCountUsers, { data: countUsers }] = useCountUsersMutation();
  const [getAverageTimeOnPages, { data: averageTimeOnPages }] = useAverageTimeOnPagesMutation();
  const [getCountUsersOnPages, { data: countUsersOnPages }] = useCountUsersOnPagesMutation();
  
  useEffect(() => {
    const dataReq = {
      leftDate: leftDate,
      rightDate: rightDate,
      intervalID: intervalId,
    };

    const dataReqPages = {
      leftDate: leftDate,
      rightDate: rightDate,
      intervalID: intervalId,
      activityIDs: activityIDs,
    };

    getAverageTime(dataReq);
    getCountUsers(dataReq);
    getAverageTimeOnPages(dataReqPages);
    getCountUsersOnPages(dataReqPages);
  }, [leftDate, rightDate, intervalId, activityIDs]);

  useEffect(() => {
    setAccordionList([
      {
        chartData: averageTime,
        title: "Среднее время нахождения пользователей на сайте",
      },
      {
        chartData: countUsers,
        title: "Количество пользователей на сайте",
      },
      {
        chartData: averageTimeOnPages,
        title: "Среднее время нахождение пользователей на страницах",
        flag: false,
      },
      {
        chartData: countUsersOnPages,
        title: "Количество пользователей на страницах",
        flag: true,
      },
    ]);
  }, [averageTime, countUsers, averageTimeOnPages]);


  return (
    <Container
      disableGutters
      sx={{ mt: "40px", textAlign: "center", alignItems: "center" }}
    >
      {accordionList?.map((item) => {
        return (
          <Accordion
            sx={{
              marginTop: "20px",
            }}
          >
            <AccordionSummary
              sx={{
                border: "1px solid grey",
                margin: "0 0 -1px 0px",
              }}
              expandIcon={
                <ArrowDropDownIcon sx={{ color: "#AEAEAE", fontSize: 40 }} />
              }
              aria-controls="panel1-content"
              id="panel1-header"
            >
              <Typography>{item.title}</Typography>
            </AccordionSummary>
            <AccordionDetails
              sx={{
                border: "1px solid grey",
                margin: "0 0 -1px 0px",
              }}
            >
              <Graph chartData={item?.chartData} flag={item?.flag}/>
            </AccordionDetails>
          </Accordion>
        );
      })}
    </Container>
  );
};

export default Graphs;

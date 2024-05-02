import { Card, CardContent, Container, Typography } from "@mui/material";
import {
  useAverageTimeAtomMutation,
  useAverageTimeOnPagesAtomMutation,
  useCountUsersAtomMutation,
  useCountUsersOnPagesAtomMutation,
} from "../../services/statsService";
import { useEffect, useState } from "react";
import { useAppSelector } from "../../store/redux";

type cardsList = {
  title: string;
  [key: string]: any;
};

const AtomStats = () => {
  const { leftDate, rightDate, intervalId, activityIDs } = useAppSelector(
    (state) => state.statsReducer
  );

  const [cardsList, setCardsList] = useState<cardsList[]>([]);
  const [getAverageTime, { data: averageTime }] = useAverageTimeAtomMutation();
  const [getCountUsers, { data: countUsers }] = useCountUsersAtomMutation();
  const [getAverageTimeOnPages, { data: averageTimeOnPages }] =
    useAverageTimeOnPagesAtomMutation();
  const [getCountUsersOnPages, { data: countUsersOnPages }] =
    useCountUsersOnPagesAtomMutation();

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
    setCardsList([
      {
        data: averageTime,
        title: "Среднее время нахождения пользователей на сайте",
        postfix: "- мин."
      },
      {
        data: countUsers,
        title: "Количество пользователей на сайте",
        postfix: "- пользователей"
      },
      {
        data: averageTimeOnPages,
        title: "Среднее время нахождения пользователей на страницах",
        postfix: "- мин."

      },
      {
        data: countUsersOnPages,
        title: "Количество пользователей на страницах",
        postfix: "- пользователей"
      },
    ]);
  }, [averageTime, countUsers, averageTimeOnPages]);

  console.log(cardsList)
  return (
    <Container
      disableGutters
      sx={{
        mt: "40px",
        textAlign: "center",
        alignItems: "center",
        flexDirection: "row",
        display: "flex",
        gap: "10px",
      }}
    >
      {cardsList?.map((item) => {
        return (
          <Card
          sx={{
            width: 285,
            p: "5px",
          }}
        >
          <CardContent
            sx={{
              p: "5px",
              "&:last-child": {
                paddingBottom: "5px",
              },
            }}
          >
            <Typography
              sx={{ fontSize: 10, textAlign: "left", m: 0 }}
              color="text.secondary"
              gutterBottom
            >
              {item?.title}
            </Typography>
            <Typography
              sx={{ fontSize: 24, textAlign: "left", m: 0 }}
              color="text.secondary"
              gutterBottom
            >
              {item?.data?.quantity && (item?.data?.quantity).toFixed(4)} {item?.data?.seconds && (item?.data?.seconds / 60).toFixed(2)} {item?.postfix}
            </Typography>
          </CardContent>
        </Card>
        )
      })}
    </Container>
  );
};

export default AtomStats;

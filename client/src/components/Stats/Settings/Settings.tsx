import { useEffect, useState } from "react";
import {
  useGetActivitesQuery,
  useGetIntervalsQuery,
} from "../../../services/statsService";
import {
  Button,
  Container,
  FormControl,
  InputLabel,
  MenuItem,
  Popover,
  Select,
  SelectChangeEvent,
  TextField,
  Typography,
} from "@mui/material";
import { DateRange } from "react-date-range";
import { ru } from "date-fns/locale";
import "react-date-range/dist/styles.css";
import "react-date-range/dist/theme/default.css";
import { useDispatch } from "react-redux";
import {
  setActivityIDs,
  setIntervalId,
  setLeftDate,
  setRightDate,
} from "../../../store/statsSlice";

const Settings = () => {
  const { data: intervals } = useGetIntervalsQuery();
  const { data: activities } = useGetActivitesQuery();
  const [active, setActive] = useState<string[]>([]);
  const [interval, setInterval] = useState("");
  const dispatch = useDispatch();

  const handleChangeInterval = (event: SelectChangeEvent) => {
    setInterval(event.target.value);
  };

  const handleChangeActivites = (event: SelectChangeEvent<any>) => {
    const {
      target: { value },
    } = event;
    setActive(
     
      typeof value === 'string' ? value.split(',') : value,
    );
  };

  const [dateInt, setDateInt] = useState([
    {
      startDate: new Date(),
      endDate: new Date(),
      key: "selection",
    },
  ]);

  const [anchorEl, setAnchorEl] = useState<HTMLDivElement | null>(null);

  const handleClick = (event: React.MouseEvent<HTMLDivElement>) => {
    setAnchorEl(event.currentTarget);
  };

  const handleClose = () => {
    setAnchorEl(null);
  };

  const open = Boolean(anchorEl);
  const id = open ? "simple-popover" : undefined;

  const create = () => {
    dispatch(setRightDate(Math.round(+new Date(dateInt[0]?.endDate) / 1000)));
    dispatch(setLeftDate(Math.round(+new Date(dateInt[0]?.startDate) / 1000)));
    dispatch(setIntervalId(interval));
    dispatch(setActivityIDs(active));
  }

  return (
    <Container
      disableGutters
      sx={{
        pt: "80px",
        textAlign: "center",
        alignItems: "center",
        flexDirection: "row",
      }}
    >
      <FormControl sx={{ minWidth: 120, flexDirection: "row", m: "10px" }}>
        <TextField
          sx={{ minWidth: 265, color: "black" }}
          label="Период"
          onClick={handleClick}
          defaultValue={
            "" ||
            `${dateInt[0].startDate.toLocaleDateString()} - ${dateInt[0].endDate.toLocaleDateString()}`
          }
          value={`${dateInt[0].startDate.toLocaleDateString()} - ${dateInt[0].endDate.toLocaleDateString()}`}
        ></TextField>
        <Popover
          id={id}
          open={open}
          anchorEl={anchorEl}
          onClose={handleClose}
          anchorOrigin={{
            vertical: "bottom",
            horizontal: "left",
          }}
        >
          <DateRange
            editableDateInputs={false}
            //@ts-ignore
            onChange={(item) => setDateInt([item?.selection])}
            moveRangeOnFirstSelection={false}
            ranges={dateInt}
            locale={ru}
          />
        </Popover>
      </FormControl>
      <FormControl sx={{ minWidth: 120, flexDirection: "row", m: "10px" }}>
        <InputLabel id="range"> Интервал </InputLabel>
        <Select
          labelId="Интервал"
          sx={{ minWidth: 265, textAlign: "left" }}
          value={interval}
          label="Интервал"
          onChange={handleChangeInterval}
        >
          {intervals?.map((item: { id: string; title: string }) => {
            return <MenuItem value={item.id}> {item.title} </MenuItem>;
          })}
        </Select>
      </FormControl>
      <FormControl sx={{ minWidth: 120, flexDirection: "row", m: "10px" }}>
        <InputLabel id="range"> Страница </InputLabel>
        <Select
          multiple
          labelId="Активность"
          sx={{ width: 265, textAlign: "left" }}
          value={active}
          label="Активность"
          onChange={handleChangeActivites}
        >
          {activities?.map((item: { id: string; title: string }) => {
            return <MenuItem value={item.id}> {item.title} </MenuItem>;
          })}
        </Select>
      </FormControl>
      <Button
        size="large"
        sx={{ 
          alignItems: "center",
          textAlign: "center",
          m: "16px",
          backgroundColor: "#FFFFFF",
          color: 'black',
          '&:hover': {
            color: 'white',
          },
        }}
        variant="contained"
        onClick={() => create()}
      >
        Построить
      </Button>
    </Container>
  );
};

export default Settings;

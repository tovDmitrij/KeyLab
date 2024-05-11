import Card from "@mui/material/Card";
import CardContent from "@mui/material/CardContent";
import Typography from "@mui/material/Typography";
import { CardActionArea } from "@mui/material";
import IconPlus from "../../Icons/IconPlus/IconPlus";

import classes from "./SwitchCard.module.scss";
import { useNavigate } from "react-router-dom";
import { useAppSelector } from "../../../store/redux";
import Preview from "../../List/SwitchesList/Preview";

const SwitchCard = () => {
  const navigate = useNavigate();
  const { switchTitle, switchTypeID } = useAppSelector(
    (state) => state.keyboardReduer
  );

  console.log(switchTitle);
  return (
    <Card className={classes.card} sx={{ borderRadius: 10 }}>
      <CardActionArea onClick={() => navigate("/constrSwitch")}>
        <CardContent className={classes.card_content}>
          {!switchTitle ? (
            <>
              <Typography sx={{ mt: 1, fontSize: 20, color: "#AEAEAE" }}>
                Выберите переключатель
              </Typography>
              <IconPlus sx={{ fontSize: 50 }} />
            </>
          ) : (
            <>
              <Preview id={switchTypeID} type="switch" width={165} height={165} />
              <Typography sx={{ mt: 1, fontSize: 20, color: "#AEAEAE" }}>
                {switchTitle}
              </Typography>
            </>
          )}
        </CardContent>
      </CardActionArea>
    </Card>
  );
};

export default SwitchCard;

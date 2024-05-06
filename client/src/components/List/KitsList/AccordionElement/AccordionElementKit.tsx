import { FC } from "react";

import ExpandMoreIcon from "@mui/icons-material/ExpandMore";
import {
  Accordion,
  AccordionDetails,
  AccordionSummary,
  Grid,
  List,
  ListItem,
  ListItemButton,
  ListItemText,
  Typography,
} from "@mui/material";
import { typeBoxesId } from "../../../../pages/ConstructorsBoxes/ConstructorsBoxes";
import { useGetDefaultKitsQuery } from "../../../../services/kitsService";

type TKits = {
  /**
   * id бокса
   */
  id?: string;
  /**
   * название бокса
   */
  title?: string;
};

type props = {
  type?: string;
  name: string;
  handleChoose: (data: TKits) => void;
  handleNew: (data: string) => void;
  boxTypeId: string;
};

const AccordionElementKit: FC<props> = ({
  type: type,
  boxTypeId: boxTypeId,
  name: name,
  handleChoose,
  handleNew,
}) => {

  const { data : kits } = useGetDefaultKitsQuery({
    page: 1,
    pageSize: 10,
    typeID: boxTypeId,
  });

  const onClick = (value: TKits) => {
    if (!value.id) return;
    handleChoose(value);
  };

  const onClickNew = () => {
    //console.log(typeBoxesId[type])
    handleNew(boxTypeId);
  };

  return (
    <Accordion>
      <AccordionSummary
        sx={{
          bgcolor: "#2A2A2A",
          color: "#FFFFFF",
        }}
        expandIcon={<ExpandMoreIcon />}
        aria-controls="panel2-content"
        id="panel2-header"
      >
        <Typography>{name}</Typography>
      </AccordionSummary>
      <AccordionDetails
        sx={{
          p: 0,
          bgcolor: "#2A2A2A",
          color: "#FFFFFF",
        }}
      >
        <Grid container direction="column">
          <List disablePadding>
            {kits?.map((value) => {
              const labelId = `checkbox-list-label-${value}`;

              return (
                <>
                  <ListItem
                    sx={{ minWidth: "100" }}
                    key={value.id}
                    disablePadding
                  >
                    <ListItemButton
                      role={undefined}
                      onClick={() => onClick(value)}
                      dense
                    >
                      <ListItemText
                        sx={{
                          textAlign: "center",
                          color: "white",
                          m: "5px",
                        }}
                        id={labelId}
                        primary={`${value.title}`}
                      />
                    </ListItemButton>
                  </ListItem>
                </>
              );
            })}
            <ListItem sx={{ minWidth: "100" }} disablePadding>
              <ListItemButton
                role={undefined}
                onClick={() => onClickNew()}
                dense
              >
                <ListItemText
                  sx={{
                    textAlign: "center",
                    color: "white",
                    m: "5px",
                  }}
                  primary={`Custom`}
                />
              </ListItemButton>
            </ListItem>
          </List>
        </Grid>
      </AccordionDetails>
    </Accordion>
  );
};

export default AccordionElementKit;

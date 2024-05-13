import { FC, useEffect } from "react";

import ArrowDropDownIcon from "@mui/icons-material/ArrowDropDown";
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
import {
  useGetAuthKitsQuery,
  useGetDefaultKitsQuery,
  useLazyGetAuthKitsQuery,
  useLazyGetDefaultKitsQuery,
} from "../../../../services/kitsService";

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
  name: string;
  handleChoose: (data: TKits) => void;
  handleNew: (data: string) => void;
  boxTypeId: string;
};

const AccordionElementKit: FC<props> = ({
  boxTypeId,
  name,
  handleChoose,
  handleNew,
}) => {
  const [getBaseKits, { data: kitsBase }] = useLazyGetDefaultKitsQuery();

  const [getAuthKits, { data: kits }]= useLazyGetAuthKitsQuery();

  const onClick = (value: TKits) => {
    if (!value.id) return;
    handleChoose(value);
  };

  const onClickNew = () => {
    handleNew(boxTypeId);
  };

  useEffect(() => {
    const data = {
      page: 1,
      pageSize: 100,
      typeID: boxTypeId,
    }
    getAuthKits(data);
    getBaseKits(data);
  }, [])

  return (
    <Accordion>
      <AccordionSummary
        sx={{
          bgcolor: "#2A2A2A",
          color: "#c1c0c0",
          borderTop: "1px solid grey",
          borderBottom: "1px solid grey",
          margin: "0 0 -1px 0px",
        }}
        expandIcon={
          <ArrowDropDownIcon sx={{ color: "#AEAEAE", fontSize: 40 }} />
        }
      >
        <Typography
          sx={{
            textAlign: "center",
            margin: "15px",
          }}
        >
          {name}
        </Typography>
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
            {kits &&
              kitsBase &&
              kitsBase?.concat(kits)?.map((value) => {
                const labelId = `checkbox-list-label-${value}`;

                return (
                  <>
                    <ListItem
                      sx={{ minWidth: "100" }}
                      key={value.id}
                      disablePadding
                    >
                      <ListItemButton
                        sx={{
                          textAlign: "center",
                          margin: "8px",
                          borderTop: "1px solid grey",
                          borderBottom: "1px solid grey",
                        }}
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
                sx={{
                  textAlign: "center",
                  margin: "8px",
                  borderTop: "1px solid grey",
                  borderBottom: "1px solid grey",
                }}
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
                  primary={`Создать`}
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

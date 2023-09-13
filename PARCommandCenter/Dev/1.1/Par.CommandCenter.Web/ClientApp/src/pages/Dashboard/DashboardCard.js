import React, { useEffect, useState } from 'react';
import PropTypes from 'prop-types';

// react-bootstrap components
import {
    Badge,
    Button,
    Card,
    Collapse,
    Form,
    InputGroup,
    Navbar,
    Nav,
    OverlayTrigger,
    Table,
    Tooltip,
    Container,
    Row,
    Col,
    Spinner,
} from "react-bootstrap";

import CountUp from 'react-countup';

import './DashboardCard.scss';

const DashboardCard = (props) => {

    const [isLoading, setIsloading] = useState(false);
    const [cardStatistics, setCardStatistics] = useState();
    const [errorsSelected, setErrorsSelected] = useState(false);
    const [warningsSelected, setWarningsSelected] = useState(false);


    const [activeTextSelected, setActiveTextSelected] = useState();
    const [activeSelected, setActiveSelected] = useState(false);

    const [additionalInfo, setAdditionalInfo] = useState();

    let startDateValue = "", endDateValue = "";
    if (props.filter.startDate && props.filter.endDate) {
        startDateValue = props.filter.startDate.format("MM/DD/YYYY");
        endDateValue =  props.filter.endDate.format("MM/DD/YYYY");
    }

    const url = props.cardCountsUrl; 

    useEffect(() => {      
        if (!url) {
            return;
        }
        setIsloading(true);

        setTimeout(() => {
            
            fetch(url, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ filter: "all", includeStatistics: false, statisticsOnly: true, dateRangeFilter: props.filter.value, startDate: startDateValue, endDate: endDateValue }),
            })
            .then(response => response.json())
            .then(data => {
                if (data && data.statistics) {
                    let myData = data.statistics;
                    myData.type = props.headerName.toLowerCase()
                    setCardStatistics(myData);
                    props.dataCallBack(myData);
                }

                if (!props.ftpServerStatusUrl) {
                    setIsloading(false);
                }
            });

            if (props.ftpServerStatusUrl) {
                setIsloading(true);

                fetch(props.ftpServerStatusUrl, {
                    method: 'GET',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    /*body: JSON.stringify(props.user.tenantIds)*/
                })
                .then(response => response.json())
                .then(data => {
                    setAdditionalInfo(data);
                    setIsloading(false);
                });

            }           
        }, 3000);
        
    }, [props.filter]);
   
    const handleAdditionalClickRequest = (additional) => {

        if (isLoading)
            return;

        if (additional) {
            if (additional.filter.indexOf("errors") > -1
                || additional.filter.indexOf("needCalibration") > -1) {
                setErrorsSelected(true);
                setWarningsSelected(false);
                setActiveSelected(false);
            } else if (additional.filter.indexOf("warnings") > -1 ) {
                setErrorsSelected(false);
                setWarningsSelected(true);
                setActiveSelected(false);
            } else if (additional.filter.indexOf("active") > -1) {
                setErrorsSelected(false);
                setWarningsSelected(false);
                setActiveSelected(true);
            }
            if (additional.type == 'scales') {

                setActiveTextSelected(additional.filter);
                console.log(additional.dataTableSubTitle)
                console.log(additional.filter)
            }
            else
            {
                setActiveTextSelected('additional.type');
            }
            props.showAdditionalClick(additional);
        }

        return;
    }

    let loaderContent;
    if (isLoading) {
        loaderContent = (
            <>
                <div className="loader">
                    <div className="lds-grid"><div></div><div></div><div></div><div></div><div></div><div></div><div></div><div></div><div></div></div>
                </div>
            </>
        );
    }

    let ftpServerStatusContent = <div className="py-1"></div>;
    let headerCssClasses = "py-3";
    if (props.ftpServerStatusUrl && props.ftpServerStatusUrl.includes("FTPServer")) {
        let status = <i className=" text-danger fas fa-exclamation-circle"></i>;
        if (additionalInfo && additionalInfo.ftpServerStatus) {
            status = <i className="text-success fas fa-check-circle"></i>;
        }

        headerCssClasses = "pt-3";
        ftpServerStatusContent = (
            <div className="align-items-center justify-content-center row text-secondary">
                <span className="pr-1">
                    {status}
                </span>
                <h6 className="m-0">FTP Server</h6>
            </div>
        );
    }

    let dangerBoxContnet = (
        <div className="col-auto" onClick={() =>
            handleAdditionalClickRequest({ type: props.headerName.toLowerCase(), title: props.headerName, dataUrl: props.errorDataUrl, filter: "errors", dataTableTitle: props.headerName, dataTableSubTitle: props.cardTitleDanger })
        }
        >
            <div className={"shadow card-dashboard border-0 mb-3 " + ((props.selected && errorsSelected) ? "selected" : "")}>
                <div className="  pt-2 pb-0 card-body">
                    <div className="row">

                        <div className="pl-2 d-flex align-items-center align-content-center col-auto">
                            <p className="font-size-13 m-0 card-text-hover font-weight-bold text-muted  col-auto p-0"><CountUp duration={5} end={(cardStatistics) ? cardStatistics.unHealthyTenantsCount : 0} /> Tenants</p>
                        </div>

                    </div>
                    <div className="p-1 row align-content-center align-items-center ">
                        <i className=" text-danger fas fa-exclamation-circle"></i>
                        <p className="pl-2  card-text-hover font-weight-bold m-0 text-left title  text-black ">{props.cardTitleDanger}</p>
                        <p className="font-weight-bold m-0 text-right col  text-black  "><CountUp duration={5} end={(cardStatistics) ? cardStatistics.unHealthyCount : 0} /></p>
                    </div>
                </div>
            </div>
        </div>
    );

    if (Array.isArray(props.cardTitleDanger)) {
        let cardTitleDangerContent = props.cardTitleDanger.map((item, index) => {
            return (
            
                <div key={index} className="p-1 row align-content-center align-items-center "
                    onClick={() =>
                        handleAdditionalClickRequest({ type: props.headerName.toLowerCase(), title: props.headerName, dataUrl: props.errorDataUrl, filter: item.filter, dataTableTitle: props.headerName, dataTableSubTitle: props.cardTitleDanger })
                    }
                >
                    <i className=" text-danger fas fa-exclamation-circle"></i>
                    <p className={"pl-2 card-text-hover font-weight-bold m-0 text-left title " + (((activeTextSelected) == item.filter && props.selected) ? " text-primary" : " text-black")}>{item.title}</p>
                    <p className="font-weight-bold m-0 text-right col  text-black  ">
                        <CountUp duration={5} end={(cardStatistics) ? cardStatistics[item.countProp] : 0} />
                    </p>
                </div>
            )
        });

        dangerBoxContnet = (<div className="col-auto">
            <div className={"shadow card-dashboard border-0 mb-3 " + ((props.selected && errorsSelected) ? "selected" : "")}>
                <div className="  pt-2 pb-0 card-body">
                    <div className="row">

                        <div className="pl-2 d-flex align-items-center align-content-center col-auto">
                            <p className="font-size-13 card-text-hover m-0 font-weight-bold text-muted  col-auto p-0"><CountUp duration={5} end={(cardStatistics) ? cardStatistics.unHealthyTenantsCount : 0} /> Tenants</p>
                        </div>

                    </div>
                    {cardTitleDangerContent}
                </div>
            </div>
        </div>
        );
    }

    return (

        <div key={props.cardIndex} className="py-2 mx-2 border-0 bg-white dashboard-card card-dash">            
            <div className={headerCssClasses + " align-items-center justify-content-center row text-secondary"} >
                <span className="pr-3">
                    <i className={props.icon}></i>
                </span>
                <h3 className="pl-1 m-0">{props.headerName}</h3>               
            </div>
            {ftpServerStatusContent}
           
            {dangerBoxContnet}
            
            {!props.errorFilter &&
                <div className="col-auto" onClick={() =>
                    handleAdditionalClickRequest({ type: props.headerName.toLowerCase(), title: props.headerName, dataUrl: props.activeDataUrl, filter: "active", dataTableTitle: props.headerName, dataTableSubTitle: props.cardTitleActive })
                }
                >
                    <div className={"shadow card-dashboard border-0 mb-3 " + ((props.selected && activeSelected) ? "selected" : "")}>
                        <div className="  pt-2 pb-0 card-body">
                            <div className="row">
                                <div className="pl-2 d-flex align-items-center align-content-center col-auto">
                                    <p className="font-size-13 m-0 font-weight-bold text-muted  col-auto p-0"><CountUp duration={5} end={(cardStatistics) ? cardStatistics.healthyTenantsCount : 0} /> Tenants</p>
                                </div>
                            </div>
                            <div className="p-1 row align-content-center align-items-center ">
                                <i className="text-success fas fa-check-circle"></i>
                                <p className="pl-2 card-text-hover font-weight-bold m-0 text-left title  text-black ">{props.cardTitleActive}</p>
                                <p className="font-weight-bold m-0 text-right col  text-black  "><CountUp duration={5} end={(cardStatistics) ? cardStatistics.healthyCount : 0} /></p>
                            </div>
                        </div>
                    </div>                    
                </div>
            }
            {loaderContent}
            </div>
    );
}

export default DashboardCard;

// Set default props
DashboardCard.defaultProps = {
    theme: "primary",   
};
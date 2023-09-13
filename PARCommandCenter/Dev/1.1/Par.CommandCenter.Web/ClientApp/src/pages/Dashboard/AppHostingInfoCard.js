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

import './DashboardCard.scss';

const $ = require('jquery');

const AppHostingInfoCard = (props) => { 

    useEffect(() => {        
        $(".hide-card").parent().hide();
    }, [props.environment]); 

    let headerCssClasses = "py-3";
    let hiddenCssClass = (props.environment
        && props.environment.toLowerCase().indexOf('prod') >= 0) ? " hide-card " : "";

    let loaderContent;
    if (props.isLoading) {
        loaderContent = (
            <>
                <div className={"loader "}>
                    <div className="lds-grid"><div></div><div></div><div></div><div></div><div></div><div></div><div></div><div></div><div></div></div>
                </div>
            </>
        );
    }

    return (

        <div className={"py-2 mx-2 border-0 bg-white dashboard-card card-dash " + hiddenCssClass}>
            <div className={headerCssClasses + " align-items-center justify-content-center row text-secondary"} >
                <span className="pr-3">
                    <i className={props.icon}></i>
                </span>
                <h3 className="pl-1 m-0">{props.headerName}</h3>
            </div>
            <Col>
                <Row className="p-1 align-content-center align-items-center">                    
                    <p className="font-size-13 pl-2 card-text-hover font-weight-bold m-0 text-left title  text-black"><span>Region: </span><span className="text-muted">{(props.cardStatistics) ? props.cardStatistics.region  : ''}</span></p>
                </Row> 
                <Row className="p-1 align-content-center align-items-center">
                    <p className="font-size-13 pl-2 card-text-hover font-weight-bold m-0 text-left title  text-black"><span>Resource Group: </span><span className="text-muted">{(props.cardStatistics) ? props.cardStatistics.resourceGroup : ''}</span></p>
                </Row> 
                <Row className="p-1 align-content-center align-items-center">
                    <p className="font-size-13 pl-2 card-text-hover font-weight-bold m-0 text-left title  text-black"><span>Server Time: </span><span className="text-muted">{(props.cardStatistics) ? props.cardStatistics.serverTime : ''}</span></p>
                </Row> 
                <Row className="p-1 align-content-center align-items-center">
                    <p className="font-size-13 pl-2 card-text-hover font-weight-bold m-0 text-left title  text-black"><span>DNS Host Name: </span><span className="text-muted">{(props.cardStatistics) ? props.cardStatistics.dnsHostName : ''}</span></p>
                </Row> 
                <Row className="p-1 align-content-center align-items-center">
                    <p className="font-size-13 pl-2 card-text-hover font-weight-bold m-0 text-left title  text-black"><span>Machine Name: </span><span className="text-muted">{(props.cardStatistics) ? props.cardStatistics.machineName : ''}</span></p>
                </Row> 
                <Row className="p-1 align-content-center align-items-center">
                    <p className="font-size-13 pl-2 card-text-hover font-weight-bold m-0 text-left title  text-black"><span>Primary SQL Time (UTC): </span><span className="text-muted">{(props.cardStatistics) ? props.cardStatistics.sqlDateTimePrimary : ''}</span></p>
                </Row> 
                <Row className="p-1 align-content-center align-items-center">
                    <p className="font-size-13 pl-2 card-text-hover font-weight-bold m-0 text-left title  text-black"><span>Secondary SQL Time (UTC): </span><span className="text-muted">{(props.cardStatistics) ? props.cardStatistics.sqlDateTimeSecondary : ''}</span></p>
                </Row>
                <Row className="p-1 align-content-center align-items-center">
                    <p className="font-size-13 pl-2 card-text-hover font-weight-bold m-0 text-left title  text-black"><span>Primary SQL Reply: </span><span className="text-muted">{(props.cardStatistics) ? props.cardStatistics.sqlMessagePrimary : ''}</span></p>
                </Row>
                <Row className="p-1 align-content-center align-items-center">
                    <p className="font-size-13 pl-2 card-text-hover font-weight-bold m-0 text-left title  text-black"><span>Secondary SQL Reply: </span><span className="text-muted">{(props.cardStatistics) ? props.cardStatistics.sqlMessageSecondary : ''}</span></p>
                </Row>
            </Col>            
            {loaderContent}
        </div>
    );
}

export default AppHostingInfoCard;

// Set default props
AppHostingInfoCard.defaultProps = {
    theme: "primary",
};
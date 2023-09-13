////import React, { useState } from 'react';
////import PropTypes from 'prop-types';

////// react-bootstrap components
////import {
////    Badge,
////    Button,
////    ButtonGroup,
////    Card,
////    Form,
////    InputGroup,
////    Navbar,
////    Nav,
////    Table,
////    Container,
////    Row,
////    Col,
////    OverlayTrigger,
////    Tooltip,
////    ListGroup
////} from "react-bootstrap";
////import { propTypes } from 'react-bootstrap/esm/Image';

//// const TenantSetupTab = (props) => {

////    const tabItems = props.tabs.map((tab) => {
////        let icon = ((tab.iconClass) && tab.iconClass.length > 0) ?  <i className={tab.iconClass} /> : undefined;
////        return (
////            <Button key={tab.value}
////                className="btn-outline "
////                type="button"
////                variant="primary"
////                active={tab.value == props.selectedValue}
////                tab-value={tab.value}
////                onClick={props.onTabItemClick}
////            >
                
////                <span className="d-flex align-middle">
////                    {icon}
////                    <span className="align-middle">{tab.name}</span>
////                </span>
////            </Button >);        
////    });

////    return (
////        <ButtonGroup className="flex-wrap">
////            {tabItems}
////        </ButtonGroup>
////    );
////};

////export default TenantSetupTab;

////TenantSetupTab.propTypes = {
////    tabs: PropTypes.arrayOf(
////        PropTypes.shape({
////            value: PropTypes.string.isRequired,
////            name: PropTypes.string.isRequired,
////            iconClass: PropTypes.string,
////        }).isRequired
////    ).isRequired,
////    selectedValue: PropTypes.string,
////    onTabItemClick: PropTypes.func.isRequired,
////}

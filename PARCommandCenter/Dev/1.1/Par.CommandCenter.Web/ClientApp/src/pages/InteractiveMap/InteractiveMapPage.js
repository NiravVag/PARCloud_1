import React, { useEffect, useState } from "react";
import * as atlas from 'azure-maps-control';

// react-bootstrap components
import {
    Badge,
    Button,
    Form,
    InputGroup,
    Navbar,
    Nav,
    Container,
    Col,
    Row,
} from "react-bootstrap";

import "azure-maps-control/dist/atlas.min.css";

import "./../../styles/map.scss";
import FilterList from "../../components/Common/FilterList";
import InteractiveMap from "./InteractiveMap";



const additionalFilterOptions = [
    { value: "interface", label: "Interface", checked: true, apiParam: "includeInterfaces" },
    { value: "router", label: "Router", checked: true, apiParam: "includeRouters"},
    { value: "controller", label: "Controller", checked: true, apiParam: "includeControllers" },
    { value: "scale", label: "Scales", checked: true, apiParam: "includeScales" },
];

function InteractiveMapPage() {

    const [selectedFilter, setSelectedFilter] = useState({ value: "past24Hours", label: "Past 24 hours" });
    const [mapUrl, setMapUrl] = useState("");

    const [mapDataURL, setMapDataURL] = useState("");

    const [interfaceFilterChecked, setInterfaceFilterChecked] = useState(true);
    const [routerFilterChecked, setRouterFilterChecked] = useState(true);
    const [controllerFilterChecked, setControllerFilterChecked] = useState(true);
    const [scaleFilterChecked, setScaleFilterChecked] = useState(true);

    const [additionalFilter, setAdditionalFilter] = useState(additionalFilterOptions);

    useEffect(() => {
        let startDateURL = "", endDateURL = "";
        if (selectedFilter.startDate && selectedFilter.endDate) {
            startDateURL = "&startDate=" + selectedFilter.startDate.format("MM/DD/YYYY");
            endDateURL = "&endDate=" + selectedFilter.endDate.format("MM/DD/YYYY");
        }

        let geoJsonAllDataURL = document.baseURI + "api/Map/Get"
            + "?dateRangeFilter=" + selectedFilter.value
            + startDateURL
            + endDateURL;

        setMapUrl(geoJsonAllDataURL);

        additionalFilter.forEach(ele => {
            geoJsonAllDataURL += "&" + ele.apiParam + "=" + ele.checked;
        });

        setMapDataURL(geoJsonAllDataURL);

        //GetMap(geoJsonAllDataURL, additionalFilter);
    }, []);

    const handleMapAdditionalFilter = (event) => {

        let filter = additionalFilter.map(f => {
            if (event.target.value.includes(f.value)) {
                let copy = {};
                Object.assign(copy, f);
                copy.checked = !f.checked;
                return copy;
            }

            return f;
        });

        setAdditionalFilter(filter);

        let geoJsonAllDataURL = mapUrl;

        filter.forEach(ele => {
            geoJsonAllDataURL += "&" + ele.apiParam + "=" + ele.checked;
        });

        setMapDataURL(geoJsonAllDataURL);

        //GetMap(geoJsonAllDataURL, filter);
    }

    const handleMapFilter = (filter) => {
        if ((filter.value != "customDate") || (filter.value == "customDate" && filter.startDate && filter.endDate)) {
            setSelectedFilter(filter);

            let startDateURL = "", endDateURL = "";
            if (filter.startDate && filter.endDate) {
                startDateURL = "&startDate=" + filter.startDate.format("MM/DD/YYYY");
                endDateURL = "&endDate=" + filter.endDate.format("MM/DD/YYYY");
            }

            let geoJsonAllDataURL = document.baseURI + "api/Map/Get"
                + "?dateRangeFilter=" + filter.value
                + startDateURL
                + endDateURL;

            setMapUrl(geoJsonAllDataURL);

            additionalFilter.forEach(ele => {
                geoJsonAllDataURL += "&" + ele.apiParam + "=" + ele.checked;
            });

            setMapDataURL(geoJsonAllDataURL);

            //GetMap(geoJsonAllDataURL, additionalFilter);
        }
    }

    return (
        <Container fluid id="interactiveMap" className="mb-2">
            <Row className="justify-content-between mt-0">
                <div className="col-md-6" >
                    <div className="d-flex " >

                        <FilterList onSelection={handleMapFilter} />

                    </div>
                </div>

                <div className="justify-content-between d-flex col-md-6" >

                    <div className="control-label" md="1">
                        <div className="btn-tenant-filter btn-filter btn-round btn btn-secondary-color font-weight-bold">Filter:</div>

                    </div>

                    <label className="align-items-center">
                        <Form.Check className="checkbox-inline"
                            type="switch"
                            defaultValue="interfaceFilter"
                            value="interfaceFilter"
                            id="interfaceFilter"
                            name="interfaceFilter"
                            className=""
                            checked={interfaceFilterChecked}
                            onChange={(event) => {
                                setInterfaceFilterChecked(!interfaceFilterChecked);
                                handleMapAdditionalFilter(event)
                            }}
                        />
                        <span className="ml-1 label-switch-span">Interface</span>
                    </label>




                    <label className="align-items-center">
                        <Form.Check className="checkbox-inline"
                            value="routerFilter"
                            id="routerFilter"
                            type="switch"
                            checked={routerFilterChecked}
                            onChange={(value) => {
                                setRouterFilterChecked(!routerFilterChecked);
                                handleMapAdditionalFilter(value);
                            }}
                        />
                        <span className="ml-1 label-switch-span">Router</span>
                    </label>


                    <label className="align-items-center">
                        <Form.Check className="checkbox-inline"
                            value="controllerFilter"
                            id="controllerFilter"
                            type="switch"
                            checked={controllerFilterChecked}
                            onChange={(value) => {
                                setControllerFilterChecked(!controllerFilterChecked);
                                handleMapAdditionalFilter(value);
                            }}
                        />
                        <span className="ml-1 label-switch-span">Controller</span>
                    </label>






                    <label className="align-items-center">
                        <Form.Check className="checkbox-inline"
                            value="scaleFilter"
                            id="scaleFilter"
                            type="switch"
                            checked={scaleFilterChecked}
                            onChange={(value) => {
                                setScaleFilterChecked(!scaleFilterChecked);
                                handleMapAdditionalFilter(value);
                            }}
                        />
                        <span className="ml-1 label-switch-span">Scale</span>
                    </label>

                </div>
            </Row>
            <Row>
                <Col className="mt-2">
                    <InteractiveMap mapDataURL={mapDataURL} timeRangeFilter={selectedFilter} startDate={selectedFilter.startDate} endDate={selectedFilter.endDate} />
                </Col>
            </Row>
        </Container>
    );
}







export default InteractiveMapPage;

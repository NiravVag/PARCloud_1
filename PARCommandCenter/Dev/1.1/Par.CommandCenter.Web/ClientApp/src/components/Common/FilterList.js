import React, { useState, useEffect} from 'react';

// react-bootstrap components
import {
    Badge,
    Button,
    Card,
    Form,
    InputGroup,
    Navbar,
    Nav,
    Table,
    Container,
    Row,
    Col,
    OverlayTrigger,
    Tooltip,
    ListGroup
} from "react-bootstrap";

// react plugin used to create DropdownMenu for selecting items
import Select from "react-select";

import moment from "moment";

// react plugin used to create datetimepicker
import ReactDatetime from "react-datetime";

import './FilterList.scss';

const filterOptions = [
    { value: "past24Hours", label: "Past 24 hours" },
    { value: "past3Days", label: "Past 3 days" },
    { value: "past7Days", label: "Past 7 days" },
    { value: "past30Days", label: "Past 30 days" },
    { value: "customDate", label: "Define custom" },
    /*{ value: "errorsOnly", label: "Critical Outages Only" },*/
];

const FilterList = (props) => {
    const [selectedFilter, setSelectedFilter] = useState(filterOptions[0]);

    const [editMode, setEditMode] = useState(false);

    const [previousSelectedFilter, setPreviousSelectedFilter] = useState();

    const [startDate, setStartDate] = useState();
    const [endDate, setEndDate] = useState();

    useEffect(() => {
        const queryParameters = new URLSearchParams(window.location.search)
        const timeRangeFilter = queryParameters.get("filter");
        let filter = {};
        if (timeRangeFilter && timeRangeFilter != null && timeRangeFilter) {
            filter = filterOptions.find(x => x.value == timeRangeFilter)
            if (filter == undefined || filter == null) {
                filter = { value: "past24Hours", label: "Past 24 hours" };
            }
            else
            {
                if (filter.value == "customDate") {
                    let startDate = queryParameters.get("startDate");                    
                    setStartDate(moment(startDate));
                    let endDate = queryParameters.get("endDate");
                    setEndDate(moment(endDate));
                    filter.startDate = moment(startDate);
                    filter.endDate = moment(endDate);
                }
            }
        }
        else {
            filter = { value: "past24Hours", label: "Past 24 hours" };
        }
        setSelectedFilter(filter);
        props.onSelection(filter);
    }, []);




    const handleEditFilter = (event) => {
        //if (selectedFilter.value == "customDate") {
        //    setSelectedFilter(previousSelectedFilter);
        //}
        if (selectedFilter.value == "customDate") {
            setStartDate(null);
            setEndDate(null);
            const filter = { value: "past24Hours", label: "Past 24 hours" }
            setSelectedFilter(filter);
            props.onSelection(filter);
        }

        setEditMode(!editMode);
    };

    const handleSelectFilter = (selectedOption) => {
        if (selectedOption.value == "customDate") {
            setPreviousSelectedFilter(selectedFilter);
        }
        //else if (selectedOption.value == "errorsOnly") {
        //    setPreviousSelectedFilter(selectedFilter);

        //    setEditMode(!editMode);

        //    let selectedFilterObj = { value: selectedFilter.value, label: selectedFilter.label, startDate: startDate, endDate: endDate };

        //    /*setSelectedFilter(selectedFilterObj);*/

        //    props.onSelection(selectedFilterObj);

        //    setCriticalOutagesOnly(true);

        //    return;
        //}
        // if the selected option is not a custom filter date range, close the edit mode.
        else {
            setEditMode(!editMode);
            setStartDate();
            setEndDate();
        }

        setSelectedFilter(selectedOption);

        props.onSelection(selectedOption);
    };

    const handleDoneChangeCustomDate = () => {
        let testStartDate = Date.parse(startDate), testEndDate = Date.parse(endDate);

        if (isNaN(testStartDate)) {
            alert("The Start date is invalid. Please enter a date in the format MM/DD/YYYY");
            return;
        }

        if (isNaN(testEndDate)) {
            alert("The End date is invalid. Please enter a date in the format MM/DD/YYYY");
            return;
        }

        if (startDate.isAfter(endDate)) {
            alert("The start date must come before the end date");
            return;
        }

        setEditMode(!editMode);

        let selectedFilterObj = { value: selectedFilter.value, label: selectedFilter.label, startDate: startDate, endDate: endDate };

        setSelectedFilter(selectedFilterObj);

        props.onSelection(selectedFilterObj);
    };


    let filterContent = <Button className="btn-secondary-color btn-round btn-wd mr-1 btn-filter" variant="default" onClick={handleEditFilter}>Time range: <span className="font-weight-bold" >{selectedFilter.label}</span> </Button>;

    // disable future dates
    const today = moment();
    const disableFutureDt = current => {
        return current.isBefore(today)
    }

    if (editMode) {
        // If the selected filter type is custom date range, display the date picker.
        if (selectedFilter.value == "customDate") {
            filterContent = (
                <>
                    <Col lg="auto">
                        <Row>
                            <Col xs="auto" className="">
                                <label className="pt-2">Start:</label>
                            </Col>
                            <Col md="4" className="">
                                <ReactDatetime
                                    inputProps={{
                                        className: "form-control",
                                        placeholder: "MM/DD/YYYY",
                                    }}
                                    timeFormat={false}
                                    onChange={(value) => setStartDate(value)}
                                    isValidDate={disableFutureDt}
                                ></ReactDatetime>
                            </Col>
                            <Col xs="auto" className="">
                                <label className="pt-2">End:</label>
                            </Col>
                            <Col md="4" className="">
                                <ReactDatetime
                                    inputProps={{
                                        className: "form-control",
                                        placeholder: "MM/DD/YYYY",
                                    }}
                                    timeFormat={false}
                                    onChange={(value) => setEndDate(value)}
                                    isValidDate={disableFutureDt}
                                ></ReactDatetime>
                            </Col>
                            <Col md="5" className="">
                                <Button className="btn-round mr-1 btn-filter" size="default" variant="primary" onClick={handleDoneChangeCustomDate}>Done</Button>
                                <Button className="btn-round mr-1 btn-filter" size="default" variant="secondary" onClick={handleEditFilter}>Cancel</Button>
                            </Col>
                        </Row>
                    </Col>
                </>
            );
        }
        // display the filter dropdown
        else {
            filterContent = (
                <>
                    <Col xs="auto">
                        <label className="pt-2">Time range:</label>
                    </Col>
                    <Col md="5">
                        <Select
                            className="react-select primary"
                            classNamePrefix="react-select"
                            name="filterOptions"
                            value={selectedFilter}
                            onChange={(value) => handleSelectFilter(value)}
                            options={filterOptions}
                        />
                    </Col>
                </>
            );
        }
    }


    // if dateRange was selected, display the date range values.
    let customDateRangeContnet;
    if (startDate && endDate && !editMode) {
        customDateRangeContnet = <Button className="btn-round btn-wd mr-1 btn-filter" variant="default">Custom ( <span className="font-weight-bold" >{startDate.format('ddd MMM DD YYYY')} - {endDate.format('ddd MMM DD YYYY')}</span> ) </Button>;
    }

    return (

        <>
            {filterContent}
            {customDateRangeContnet}

        </>

    );
}

export default FilterList
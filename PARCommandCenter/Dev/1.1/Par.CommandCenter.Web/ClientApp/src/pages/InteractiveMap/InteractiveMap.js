import React, { useEffect, useState } from "react";
import * as atlas from 'azure-maps-control';

import "azure-maps-control/dist/atlas.min.css";

import "./../../styles/map.scss";

function InteractiveMap(props) {



    useEffect(() => {
        if (props.mapDataURL && props.mapDataURL.length > 0) {
            GetMap(props.mapDataURL, props.timeRangeFilter, props.startDate, props.endDate);
        }

    }, [props.mapDataURL, props.timeRangeFilter, props.startDate, props.endDate]);

    return (
        <div id="myMap" className="full-screen-map"></div>
    );
}

var map;
function GetMap(geoJsonAllDataURL, timeRangeFilter, startDate, endDate) {
    //console.log('geoJsonAllDataURL', geoJsonAllDataURL)
    //console.log('geoJsonAllDataURL timeRangeFilter', timeRangeFilter)
    if (map != null) {
        map.dispose();
    }

    //Initialize a map instance.
    map = new atlas.Map('myMap', {
        center: [-97, 39],
        zoom: 4,
        /* style: 'night',*/
        view: 'Auto',

        //Add authentication details for connecting to Azure Maps.
        authOptions: {
            //Use Azure Active Directory authentication.
            authType: 'subscriptionKey',
            subscriptionKey: "3jAfZUHexe63EPyQ4vdbh47aRZ90uv8Zx9Nxp0lhi2E", //Your Azure Active Directory client id for accessing your Azure Maps account.           
        }
    });

    var entityTypes = ['Healthy', 'Errors', 'Total Router Health Checks', 'Total Controller Health Checks', 'Total Order Interface Health Checks', 'Total Inventory Interface Health Checks',
        'Total Scale Health Checks'];

    var popup;

    //Wait until the map resources are ready.
    map.events.add('ready', function () {
        //Create a reusable popup.
        popup = new atlas.Popup();

        //Create a data source and add it to the map.
        var datasource = new atlas.source.DataSource(null, {
            cluster: true,

            //The radius in pixels to cluster points together.
            clusterRadius: 50,

            //Calculate counts for each entity type in a cluster as custom aggregate properties.
            clusterProperties: {
                'Errors': ['+', ['case', ['==', ['get', 'PointType'], 'Error'], 1, 0]],
                'Healthy': ['+', ['case', ['==', ['get', 'PointType'], 'Healthy'], 1, 0]],
                'Total Router Health Checks': ['+', ['case', ['==', ['get', 'HealthCheckType'], 'RouterHealthCheck'], 1, 0]],
                'Total Controller Health Checks': ['+', ['case', ['==', ['get', 'HealthCheckType'], 'ControllerHealthCheck'], 1, 0]],
                'Total Order Interface Health Checks': ['+', ['case', ['==', ['get', 'HealthCheckType'], 'OrderInterfaceHealthCheck'], 1, 0]],
                'Total Inventory Interface Health Checks': ['+', ['case', ['==', ['get', 'HealthCheckType'], 'InventoryInterfaceHealthCheck'], 1, 0]],
                'Total Scale Health Checks': ['+', ['case', ['==', ['get', 'HealthCheckType'], 'ScaleHealthCheck'], 1, 0]],
                'TotalCount': ['+', ['case', ['==', ['get', 'PointType'], 'Healthy'], 1, 0], ['case', ['==', ['get', 'PointType'], 'Error'], 1, 0]],
                'tenant_name': ['coalesce', ['get', 'TenantName'], ['get', 'TenantName']],
                'facility_name': ['coalesce', ['get', 'FacilityName'], ['get', 'FacilityName']],
            }
        });

        map.sources.add(datasource);

        //Create a bubble layer for rendering clustered data points.
        var clusterBubbleLayer = new atlas.layer.BubbleLayer(datasource, null, {
            radius: 30,
            //Scale the size of the clustered bubble based on the number of points in the cluster.
            //radius: [
            //    'step',
            //    ['get', 'point_count'],
            //    30,         //Default of 20 pixel radius.
            //  /*  1, 30,    //If point_count >= 100, radius is 30 pixels.*/
            //    1, 40     //If point_count >= 750, radius is 40 pixels.
            //],
            /* color: 'rgba(255,0,0,0.8)',*/
            //Change the color of the cluster based on the value on the point_cluster property of the cluster.
            color: [
                'step',
                ['get', 'Errors'],
                'rgba(0,255,0,0.8)',            //Default to green. 
                /* 1, 'rgba(255,255,0,0.8)', */    //If the point_count >= 100, color is yellow.
                1, 'rgba(255,0,0,0.8)'        //If the point_count >= 100, color is red.
            ],
            strokeWidth: 0,
            filter: ['has', 'Errors'] //Only rendered data points which have a point_count property, which clusters do.
        });

        //Create a layer to render the individual locations.
        var singleBubbleLayer = new atlas.layer.BubbleLayer(datasource, null, {
            radius: 30,
            strokeWidth: 0,
            color: [
                'case',
                ['==', ['get', 'PointType'], 'Healthy'], 'rgba(0,255,0,0.8)',
                ['==', ['get', 'PointType'], 'Error'], 'rgba(255,0,0,0.8)',
                'black'
            ],
            filter: ['!', ['has', 'point_count']] //Filter out clustered points from this layer.
        })


        //Add a click event to the layer so a popup can be displayed to show details about the cluster.
        map.events.add('click', clusterBubbleLayer, clusterClicked);
        map.events.add('click', singleBubbleLayer, clusterClicked);


        //Add the clusterBubbleLayer and two additional layers to the map.
        map.layers.add([
            clusterBubbleLayer,

            //Create a symbol layer to render the count of locations in a cluster.
            new atlas.layer.SymbolLayer(datasource, null, {
                iconOptions: {
                    image: 'none' //Hide the icon image.
                },
                textOptions: {
                    textField: ['get', 'point_count_abbreviated'],
                    offset: [0, 0.4],
                    color: 'white'
                },
                filter: ['has','point_count']
            }),

            singleBubbleLayer,
            //Create a symbol layer to render the count of locations in a cluster.
            new atlas.layer.SymbolLayer(datasource, null, {
                iconOptions: {
                    image: 'none' //Hide the icon image.
                },
                textOptions: {
                    textField: '1',
                    offset: [0, 0.4],
                    color: 'white'
                },
                filter: ['!', ['has', 'point_count']] //Filter out clustered points from this layer.
            }),
        ]);

        //Import the GeoJSON data into the data source.
        datasource.importDataFromUrl(geoJsonAllDataURL);
    });

    function singleClusterPopup(et, properties) {
        if (et == "Healthy" && properties.PointType == "Healthy") {
            return 1;
        }
        else if (et == "Errors" && properties.PointType == "Error") {
            return 1;
        }
        else if (et == "Total Router Health Checks" && properties.HealthCheckType == "RouterHealthCheck")
            return 1;
        else if (et == "Total Controller Health Checks" && properties.HealthCheckType == "ControllerHealthCheck")
            return 1;
        else if (et == "Total Order Interface Health Checks" && properties.HealthCheckType == "OrderInterfaceHealthCheck")
            return 1;
        else if (et == "Total Inventory Interface Health Checks" && properties.HealthCheckType == "InventoryInterfaceHealthCheck")
            return 1;
        else if (et == "Total Scale Health Checks" && properties.HealthCheckType == "ScaleHealthCheck")
            return 1;
        else
            return 0;
    }
    function clusterClicked(e) {
        if (e && e.shapes && e.shapes.length > 0 && e.shapes[0].properties && e.shapes[0].properties.cluster) {

            // popup = new atlas.Popup();
            //Get the clustered point from the event.
            var cluster = e.shapes[0];


            var html = ['<div style="padding:10px;">'];

            //html.push(`<b>Cluster size: ${cluster.properties.point_count_abbreviated} entities</b><br/><br/>`);
            html.push(`<b>${cluster.properties.point_count_abbreviated} Total Health Check Records</b><br/>`);

            html.push(`&#9;<b>Tenant Name: ${cluster.properties.tenant_name} </b><br/>`);

            html.push(`&#9;<b>Facility Name: ${cluster.properties.facility_name} </b><br/>`);

            //console.log('timeRangeFilter', timeRangeFilter)

            entityTypes.forEach(et => {
                //html.push(`${et}: ${cluster.properties[et]}<br/>`);

                if (timeRangeFilter.value != "customDate") {
                    if (et == "Total Router Health Checks") {
                        html.push(`${et}: <a href="/dashboard?filter=${timeRangeFilter.value}">${cluster.properties[et]}</a><br/>`);
                    }
                    else if (et == "Total Controller Health Checks") {
                        html.push(`${et}: <a href="/dashboard?filter=${timeRangeFilter.value}">${cluster.properties[et]}</a><br/>`);
                    }
                    else if (et == "Total Order Interface Health Checks") {
                        html.push(`${et}: <a href="/dashboard?filter=${timeRangeFilter.value}">${cluster.properties[et]}</a><br/>`);
                    }
                    else if (et == "Total Inventory Interface Health Checks") {
                        html.push(`${et}: <a href="/dashboard?filter=${timeRangeFilter.value}">${cluster.properties[et]}</a><br/>`);
                    }
                    else if (et == "Total Scale Health Checks") {
                        html.push(`${et}: <a href="/dashboard?filter=${timeRangeFilter.value}">${cluster.properties[et]}</a><br/>`);
                    }
                    else {
                        html.push(`${et}: ${cluster.properties[et]}<br/>`);
                    }
                }
                else {
                    if (et == "Total Router Health Checks") {
                        html.push(`${et}: <a href="/dashboard?filter=${timeRangeFilter.value + "&startDate=" + startDate.format("MM/DD/YYYY") + "&endDate=" + endDate.format("MM/DD/YYYY")}">${cluster.properties[et]}</a><br/>`);
                    }
                    else if (et == "Total Controller Health Checks") {
                        html.push(`${et}: <a href="/dashboard?filter=${timeRangeFilter.value + "&startDate=" + startDate.format("MM/DD/YYYY") + "&endDate=" + endDate.format("MM/DD/YYYY")}">${cluster.properties[et]}</a><br/>`);
                    }
                    else if (et == "Total Order Interface Health Checks") {
                        html.push(`${et}: <a href="/dashboard?filter=${timeRangeFilter.value + "&startDate=" + startDate.format("MM/DD/YYYY") + "&endDate=" + endDate.format("MM/DD/YYYY")}">${cluster.properties[et]}</a><br/>`);
                    }
                    else if (et == "Total Inventory Interface Health Checks") {
                        html.push(`${et}: <a href="/dashboard?filter=${timeRangeFilter.value + "&startDate=" + startDate.format("MM/DD/YYYY") + "&endDate=" + endDate.format("MM/DD/YYYY")}">${cluster.properties[et]}</a><br/>`);
                    }
                    else if (et == "Total Scale Health Checks") {
                        html.push(`${et}: <a href="/dashboard?filter=${timeRangeFilter.value + "&startDate=" + startDate.format("MM/DD/YYYY") + "&endDate=" + endDate.format("MM/DD/YYYY")}">${cluster.properties[et]}</a><br/>`);
                    }
                    else {
                        html.push(`${et}: ${cluster.properties[et]}<br/>`);
                    }                    
                }
            })

            html.push('</div>');

            //Update the options of the popup and open it on the map.
            popup.setOptions({
                position: cluster.geometry.coordinates,
                content: html.join('')
            });

            popup.open(map);
        }
        else {

            var cluster = e.shapes[0];
            //console.log(cluster);
            var html = ['<div style="padding:10px;">'];

            html.push(`<b>1 Total Health Check Records</b><br/>`);

            html.push(`&#9;<b>Tenant Name: ${cluster.data.properties.TenantName} </b><br/>`);

            html.push(`&#9;<b>Facility Name: ${cluster.data.properties.FacilityName} </b><br/>`);


            //Loop though each entity type get the count from the clusterProperties of the cluster.
            entityTypes.forEach(et => {
                //html.push(`${et}: ${singleClusterPopup(et, cluster.data.properties)}<br/>`);

                if (et == "Total Router Health Checks") {
                    html.push(`${et}: <a href="/dashboard?filter=${timeRangeFilter.value}">${singleClusterPopup(et, cluster.data.properties)}</a><br/>`);
                }
                else if (et == "Total Controller Health Checks") {
                    html.push(`${et}: <a href="/dashboard?filter=${timeRangeFilter.value}">${singleClusterPopup(et, cluster.data.properties)}</a><br/>`);
                }
                else if (et == "Total Order Interface Health Checks") {
                    html.push(`${et}: <a href="/dashboard?filter=${timeRangeFilter.value}">${singleClusterPopup(et, cluster.data.properties)}</a><br/>`);
                }
                else if (et == "Total Inventory Interface Health Checks") {
                    html.push(`${et}: <a href="/dashboard?filter=${timeRangeFilter.value}">${singleClusterPopup(et, cluster.data.properties)}</a><br/>`);
                }
                else if (et == "Total Scale Health Checks") {
                    html.push(`${et}: <a href="/dashboard?filter=${timeRangeFilter.value}">${singleClusterPopup(et, cluster.data.properties)}</a><br/>`);
                }
                else {
                    html.push(`${et}: ${singleClusterPopup(et, cluster.data.properties)}<br/>`);
                }
            })

            html.push('</div>');
            //Update the options of the popup and open it on the map.
            popup.setOptions({
                position: cluster.data.geometry.coordinates,
                content: html.join('')
            });

            popup.open(map);
        }
    }
}


//function clusterClicked(e) {
//    if (e && e.shapes && e.shapes.length > 0 && e.shapes[0].properties.cluster) {
//        //Get the clustered point from the event.
//        var cluster = e.shapes[0];

//        //Get the cluster expansion zoom level. This is the zoom level at which the cluster starts to break apart.
//        datasource1.getClusterExpansionZoom(cluster.properties.cluster_id).then(function (zoom) {

//            //Update the map camera to be centered over the cluster. 
//            map.setCamera({   
//                center: cluster.geometry.coordinates,
//                zoom: zoom,
//                type: 'ease',
//                duration: 200
//            });
//        });
//    }
//}


export default InteractiveMap;

import { HttpClient, HttpHandler } from '@angular/common/http';
import { Component, Inject, OnInit, OnDestroy, Input, Output, EventEmitter } from '@angular/core';
import { Heatmap } from 'heatmap.js';
import { interval, Observable, repeat, timeout, timer } from 'rxjs';
// import * as Hm from 'heatmap.js';
declare let L:any;

// declare const Test:any;

// import 'heatmap.js';


@Component({
    selector: 'app-map',
    templateUrl: './map.component.html',
    styleUrls: ['map.component.css'],
})

export class MapComponent implements OnInit{ 
    public realEstates: RealEstate[] = [];
    public http: HttpClient;
    public baseURL: string;

    public heatmapData: Array<any> = [];

    constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        http.get<RealEstate[]>(baseUrl + 'domusdatabase/getall').subscribe(result => {
          this.realEstates = result; this.realEstates.forEach(r => {
            this.heatmapData.push({lat: r.lat, lng: r.lon, count: r.sqmPrice})
          })
        });

        this.http = http;
        this.baseURL = baseUrl;
    }
    
    ngOnInit() : void
    {   
        // map-baselayer, write into class property, construct baselayer in costructor
        var baseLayer = L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
            attribution: '© OpenStreetMap contributors',
        })

        // initialize heatmap with config "cfg"
        var heatmapLayer = new HeatmapOverlay(cfg)

        let map = L.map('map', {
            center: new L.LatLng(60, 12),
            zoom:6,
            layers: [baseLayer, heatmapLayer]
        }); 

        setTimeout(() => {
            heatmapLayer.setData({
                max: 5000,
                data: this.heatmapData,
                min: 0 
            });
        }, 500);

        interval(5000).subscribe(x => {
            map.removeLayer(heatmapLayer);
            map.addLayer(heatmapLayer);
            heatmapLayer.setData({
                max: 5000,
                data: this.heatmapData,
                min: 0 
            });
        });
        
    }
    
    getData() : Observable<any>
    {
        this.http.get<RealEstate[]>(this.baseURL + 'domusdatabase/getall').subscribe(result => {
            this.realEstates = result;
        });

        return new Observable();
        
    }
}

interface RealEstate {
    finnkodeId: number;
    price: number;
    sqm: number;
    sqmPrice: number;
    address: string
    lat: number;
    lon: number;
}

var cfg = {
    // radius should be small ONLY if scaleRadius is true (or small radius is intended)
    // if scaleRadius is false it will be the constant radius used in pixels
    "radius": 0.1,
    "maxOpacity": .4,
    // scales the radius based on map zoom
    "scaleRadius": true,
    // if set to false the heatmap uses the global maximum for colorization
    // if activated: uses the data maximum within the current map boundaries
    //   (there will always be a red spot with useLocalExtremas true)
    "useLocalExtrema": true,
    // which field name in your data represents the latitude - default "lat"
    latField: 'lat',
    // which field name in your data represents the longitude - default "lng"
    lngField: 'lng',
    // which field name in your data represents the data value - default "value"
    valueField: 'count',

    // gradient: {
    //     '0': '#ff00ff',
    //     '.5': '#ff0000',
    //     '1': '#ffaaff',
    //     '.8': '#ffff00' 

    // }
  };

var testData = {
max: 20,
data: [{lat: 10, lng: 10, count: 24},{lat: 13, lng: 13, count: 1}],
min: 0
};

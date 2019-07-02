//
//  ViewController.swift
//  SVRFFrameworkSetup
//
//  Created by Andrei Evstratenko on 11/5/18.
//  Copyright © 2018 SVRF, Inc. All rights reserved.
//

import UIKit
import ARKit

// Do not forget import SvrfSDK
import SvrfSDK

class ViewController: UIViewController {
    
    // The scene view which will handle face tracking and applying Face Filters
    @IBOutlet private weak var sceneView: ARSCNView!
    
    // The search bar provides to enter search query
    @IBOutlet private weak var searchBar: UISearchBar!
    
    // The search results collection view
    @IBOutlet private weak var searchCollectionView: SearchCollectionView!
    
    // Create an activity indicator for when we are loading search results
    @IBOutlet private weak var activityIndicator: UIActivityIndicatorView!
    
    // Label saying no results were found
    @IBOutlet private weak var noResultsLabel: UILabel!
    
    // Reset button which will appear when a face filter is applied
    @IBOutlet private weak var resetButton: UIButton!

    // This will control which Face Filter is being used
    private let contentUpdater = VirtualContentUpdater()
    
    // Init Remote Face Filter
    private let remoteFaceFilter = RemoteFaceFilter()
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        // Set the collection view's delegate
        searchCollectionView.customDelegate = self
        
        // Set the scene view's delegate
        sceneView.delegate = contentUpdater
        
        // enable default lighting settings, allows PBR metallics to be properly lit
        sceneView.autoenablesDefaultLighting = true
        
        // automatically update lighting
        sceneView.automaticallyUpdatesLighting = true
        
        // Set the filter's delegate
        remoteFaceFilter.delegate = self
        
        // Set remoteFaceFilter to contentUpdater
        contentUpdater.virtualFaceNode = remoteFaceFilter
    }
    
    override func viewWillAppear(_ animated: Bool) {
        super.viewWillAppear(animated)
        
        // Create a session configuration
        let configuration = ARFaceTrackingConfiguration()
        
        // Run the scene view's session
        sceneView.session.run(configuration)
        
        // Keep the screen on
        UIApplication.shared.isIdleTimerDisabled = true
    }
    
    override func viewWillDisappear(_ animated: Bool) {
        super.viewWillDisappear(animated)
        
        // Pause the scene view's session
        sceneView.session.pause()
        
        // Turn “Keep the screen on” off
        UIApplication.shared.isIdleTimerDisabled = false
    }

    @IBAction func resetButtonClicked(_ sender: UIButton) {
        
        // Remove any existing face filter from the SCNScene
        remoteFaceFilter.resetFaceFilters()

        // Hide the resetButton
        resetButton.isHidden = true
    }
}

// Extension that has UISearchBarDelegate's methods
extension ViewController: UISearchBarDelegate {
    
    func searchBarCancelButtonClicked(_ searchBar: UISearchBar) {
        
        // Hide searchCollectionView
        searchCollectionView.isHidden = true
        
        // Hide noResultsLabel
        self.noResultsLabel.isHidden = true
        
        // Reset text in searchBar
        searchBar.text = ""
        
        // Hide cancel button in searchBar
        searchBar.showsCancelButton = false
        
        // Hide keyboard
        searchBar.resignFirstResponder()
    }
    
    func searchBarTextDidBeginEditing(_ searchBar: UISearchBar) {
        
        // Show cancel button in searchBar
        searchBar.showsCancelButton = true
    }
    
    func searchBar(_ searchBar: UISearchBar, textDidChange searchText: String) {
        
        if searchText == "" {

            // Hide searchCollectionView
            searchCollectionView.isHidden = true

            // Setup the searchCollectionView with an empty array
            searchCollectionView.setupWith(mediaArray: [])
            
            // Reload searchCollectionView to refresh data
            searchCollectionView.reloadData()
        }
        
        // Hide noResultsLabel
        self.noResultsLabel.isHidden = true
    }

    func searchBarSearchButtonClicked(_ searchBar: UISearchBar) {
        
        // Show the activity indicator
        activityIndicator.startAnimating()
        
        // Hide keyboard
        searchBar.resignFirstResponder()
        
        // Unwrap optional searchBar.text
        guard let query = searchBar.text else {
            return
        }
        
        // Limit search to 3D face filters
        let searchOptions = SvrfOptions(type: [._3d], category: .filters)
        
        // Search content via SvrfSDK
        _ = SvrfSDK.search(query: query, options: searchOptions, onSuccess: { mediaArray, _ in

            // Hide activity indicator
            self.activityIndicator.stopAnimating()
            
            // Setup the searchCollectionView with a mediaArray
            self.searchCollectionView.setupWith(mediaArray: mediaArray)
            
            // Reload searchCollectionView to refresh data
            self.searchCollectionView.reloadData()
            
            // Show searchCollectionView
            self.searchCollectionView.isHidden = false
            
            if mediaArray.isEmpty {
                // Show noResultsLabel
                self.noResultsLabel.isHidden = false
            }
        }) { error in
            print("Error: \(error.svrfDescription ?? "(no description)")")
            // Hide the activity indicator
            self.activityIndicator.stopAnimating()
        }
    }
}

extension ViewController: SearchCollectionViewDelegate {
   
    func mediaSelected(media: SvrfMedia) {
        
        // Remove any existing face filter from the SCNScene
        remoteFaceFilter.resetFaceFilters()
        
        // Show the activity indicator
        activityIndicator.startAnimating()
        
        // Load Face Filter with a media
        remoteFaceFilter.loadFaceFilter(media: media, sceneView: sceneView)
        
        // Hide resetButton
        resetButton.isHidden = false
        
        // Hide searchCollectionView
        self.searchCollectionView.isHidden = true
    }
}

extension ViewController: RemoteFaceFilterDelegate {
    
    func faceFilterLoaded() {
        
        // Hide the activityIndicator
        activityIndicator.stopAnimating()
    }
}

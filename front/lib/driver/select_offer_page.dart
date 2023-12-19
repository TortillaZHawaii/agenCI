import 'dart:convert';

import 'package:agenci/driver/offer_tile.dart';
import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;

class SelectOfferPage extends StatelessWidget {
  const SelectOfferPage({
    super.key,
    required this.driverId,
    required this.start,
    required this.end,
    required this.latitude,
    required this.longitude,
  });

  final String driverId;
  final DateTime start;
  final DateTime end;
  final double latitude;
  final double longitude;

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text("Select Offer"),
      ),
      body: FutureBuilder<List<Map<String, dynamic>>>(
        future: _fetchOffers(),
        builder: (context, snapshot) {
          if (snapshot.hasError) {
            return const Text("Error");
          }

          if (!snapshot.hasData) {
            return const Center(
              child: CircularProgressIndicator(),
            );
          }

          final offers = snapshot.data!;

          return ListView.separated(
            separatorBuilder: (context, index) => const Divider(),
            itemCount: offers.length,
            itemBuilder: (context, index) {
              final offer = offers[index];
              final parkingName = offer["key"] as String;
              final price = offer["price"] as num;
              final address = offer["address"] as String;
              return OfferTile(
                parkingName: parkingName,
                parkingAddress: address,
                price: price,
                onTap: () async {
                  await showDialog(
                    context: context,
                    builder: (context) {
                      return AlertDialog(
                        title: const Text("Confirm"),
                        content: Text("Are you sure you want to park on"
                            " $parkingName? You will be charged "
                            "$price\$ immediately."),
                        actions: [
                          TextButton(
                            onPressed: () {
                              Navigator.pop(context);
                            },
                            child: const Text("Cancel"),
                          ),
                          TextButton(
                            onPressed: () async {
                              final parkingId = offer["key"] as String;
                              final url = Uri.parse(
                                "${const String.fromEnvironment("API_BASE_URL")}"
                                "parkings/$parkingId/reserve",
                              );
                              final response = await http.post(
                                url,
                                body: {
                                  "driverId": driverId,
                                },
                              );
                              Navigator.pop(context);
                              Navigator.pop(context);

                              if (response.statusCode != 200) {
                                ScaffoldMessenger.of(context).showSnackBar(
                                  const SnackBar(
                                    content: Text("Failed to reserve"),
                                  ),
                                );
                              } else {
                                ScaffoldMessenger.of(context).showSnackBar(
                                  const SnackBar(
                                    content: Text("Reserved"),
                                  ),
                                );
                              }
                            },
                            child: const Text("Confirm"),
                          ),
                        ],
                      );
                    },
                  );
                },
              );
            },
          );
        },
      ),
    );
  }

  Future<List<Map<String, dynamic>>> _fetchOffers() async {
    const String apiBaseUrl = String.fromEnvironment("API_BASE_URL");
    final url = Uri.parse(
      '${apiBaseUrl}drivers/$driverId/parkings?'
      'start=${start.toUtc().toIso8601String()}&'
      'end=${end.toUtc().toIso8601String()}&'
      'latitude=$latitude&'
      'longitude=$longitude',
    );

    final response = await http.get(url);

    if (response.statusCode != 200) {
      throw Exception("Failed to fetch offers");
    }

    final body = jsonDecode(response.body) as List<dynamic>;

    return body.cast<Map<String, dynamic>>();
  }
}

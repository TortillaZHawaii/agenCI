import 'dart:convert';

import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;
import 'package:intl/intl.dart';

class ParkingHomePage extends StatelessWidget {
  const ParkingHomePage({
    super.key,
    required this.parkingId,
  });

  final String parkingId;

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text("Parking $parkingId"),
      ),
      body: _HistoryPageBody(parkingId: parkingId),
    );
  }
}

class _HistoryPageBody extends StatefulWidget {
  const _HistoryPageBody({super.key, required this.parkingId});
  final String parkingId;
  static const String apiBaseUrl = String.fromEnvironment("API_BASE_URL");

  @override
  State<_HistoryPageBody> createState() => __HistoryPageBodyState();
}

class __HistoryPageBodyState extends State<_HistoryPageBody> {
  @override
  Widget build(BuildContext context) {
    return FutureBuilder<Map<String, dynamic>>(
      future: _fetchHistory(),
      builder: (context, snapshot) {
        if (snapshot.hasError) {
          return const Text("Error");
        }

        if (!snapshot.hasData) {
          return const Center(
            child: CircularProgressIndicator(),
          );
        }

        final List<dynamic> history =
            snapshot.data!["reservations"] as List<dynamic>;

        final df = DateFormat("MMM dd HH:mm");

        return ListView.separated(
          separatorBuilder: (context, index) => const Divider(),
          itemCount: history.length,
          itemBuilder: (context, index) {
            final item = history[index] as Map<String, dynamic>;

            final startFormatted = df.format(
              DateTime.parse(item["start"] as String),
            );
            final endFormatted = df.format(
              DateTime.parse(item["end"] as String),
            );
            final driverKey = item["driverKey"] as String;
            final title = "$driverKey";

            final price = item["price"] as num;
            final priceFormatted = "${price.toStringAsFixed(2)} \$";

            final subtitle = "From $startFormatted to $endFormatted";

            return ListTile(
              title: Text(title),
              subtitle: Text(subtitle),
              trailing: Text(
                priceFormatted,
                style: const TextStyle(
                  fontWeight: FontWeight.normal,
                  fontSize: 16,
                ),
              ),
            );
          },
        );
      },
    );
  }

  Future<Map<String, dynamic>> _fetchHistory() async {
    final response = await http.get(
      Uri.parse(
        "${_HistoryPageBody.apiBaseUrl}parkings/${widget.parkingId}",
      ),
    );

    if (response.statusCode != 200) {
      throw Exception("Failed to fetch history");
    }

    final json = jsonDecode(response.body) as Map<String, dynamic>;
    return json;
  }
}

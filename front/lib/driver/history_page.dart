import 'dart:convert';

import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;
import 'package:intl/intl.dart';

class HistoryPage extends StatelessWidget {
  const HistoryPage({
    super.key,
    required this.driverId,
  });

  final String driverId;

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text("History"),
      ),
      body: _HistoryPageBody(
        driverId: driverId,
      ),
    );
  }
}

class _HistoryPageBody extends StatefulWidget {
  const _HistoryPageBody({super.key, required this.driverId});

  final String driverId;
  static const String apiBaseUrl = String.fromEnvironment("API_BASE_URL");

  @override
  State<_HistoryPageBody> createState() => __HistoryPageBodyState();
}

class __HistoryPageBodyState extends State<_HistoryPageBody> {
  @override
  Widget build(BuildContext context) {
    return FutureBuilder<List<Map<String, dynamic>>>(
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

        final history = snapshot.data!;

        final df = DateFormat("MMM dd HH:mm");

        return ListView.separated(
          separatorBuilder: (context, index) => const Divider(),
          itemCount: history.length,
          itemBuilder: (context, index) {
            final item = history[index];

            final startFormatted = df.format(
              DateTime.parse(item["start"] as String),
            );
            final endFormatted = df.format(
              DateTime.parse(item["end"] as String),
            );
            final parkingKey = item["key"] as String;
            final parkingAddress = item["address"] as String;
            final title = "$parkingKey $parkingAddress";

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

  Future<List<Map<String, dynamic>>> _fetchHistory() async {
    final response = await http.get(
      Uri.parse(
        "${_HistoryPageBody.apiBaseUrl}drivers/${widget.driverId}/history",
      ),
    );

    if (response.statusCode != 200) {
      throw Exception("Failed to fetch history");
    }

    final json = jsonDecode(response.body) as List<dynamic>;
    return json.cast<Map<String, dynamic>>();
  }
}
